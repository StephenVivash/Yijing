#:project ../EegML/EegML.csproj
#:property TargetFramework=net11.0
#:property PublishAot=false

using Microsoft.ML;
using Microsoft.ML.Data;
using Model = EegML.EegML;

// Run from the repository root with dotnet run --file tools/TrainStephenV.cs.
const string trainingCsv = "Yijing.py/Stephen V.csv";
const string referenceModel = "Yijing.maui/Resources/Raw/Stephen V.mlnet";
const string defaultOutput = "EegML/Stephen V.mlnet";
if (args.Length > 1)
    throw new ArgumentException("Pass at most one argument for the output model path.");

string output = Path.GetFullPath(args.Length == 1 ? args[0] : defaultOutput);
if (output.Equals(Path.GetFullPath(referenceModel), StringComparison.OrdinalIgnoreCase))
    throw new ArgumentException("Choose an output path separate from the reference model.");

var ml = new MLContext();
var csv = Model.LoadIDataViewFromFile(ml, trainingCsv, ',', hasHeader: true, allowQuoting: false);
string[] header = File.ReadLines(trainingCsv).FirstOrDefault()?.Split(',') ?? [];
if (!header.SequenceEqual(csv.Schema.Select(column => column.Name)))
    throw new InvalidDataException("Run Stephen V.ipynb to export the current 21-column training CSV.");

var rows = ml.Data.CreateEnumerable<Model.ModelInput>(csv, reuseRowObject: false).ToArray();
var data = ml.Data.LoadFromEnumerable(rows);
var reference = ml.Model.Load(referenceModel, out _);
var scored = reference.Transform(data);
double[] scores = scored.GetColumn<float>("Score").Select(score => (double)score).ToArray();
if (scores.Length < 2 || scores.Any(score => !double.IsFinite(score)) ||
    rows.Any(row => !float.IsFinite(row.Prediction)))
{
    throw new InvalidDataException("At least two training rows with finite scores are required.");
}

// Preserve the original band weights and fit a new offset and scale to the Python targets.
double scoreMean = scores.Average();
double targetMean = rows.Average(row => (double)row.Prediction);
double variance = scores.Sum(score => Math.Pow(score - scoreMean, 2));
double covariance = scores.Zip(rows, (score, row) =>
    (score - scoreMean) * (row.Prediction - targetMean)).Sum();
if (variance == 0 || covariance <= 0)
    throw new InvalidDataException("The training targets must increase with the reference scores.");

double scale = covariance / variance;
double offset = targetMean - scale * scoreMean;
string expression = FormattableString.Invariant(
    $"x => max(0f, min(1f, ({offset:F9}f) + {scale:F9}f * x))");
var calibration = ml.Transforms.Expression("Score", expression, "Score").Fit(scored);
var model = reference.Append(calibration);

string temporary = output + "." + Guid.NewGuid().ToString("N") + ".tmp";
try
{
    ml.Model.Save(model, data.Schema, temporary);
    File.Move(temporary, output, overwrite: true);
}
finally
{
    File.Delete(temporary);
}

Console.WriteLine($"Calibrated from {rows.Length} synthetic rows. Wrote {output}");
