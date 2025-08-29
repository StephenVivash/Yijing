
class Sequences:

	#ValueType [ Bit = 0, Line = 1, Duogram = 2, Trigram = 3, Hexagram = 4 ]

	TextType = [ "Text", "Image", "Judgement", "Line1", "Line2", "Line3", "Line4", "Line5", "Line6" ]
	DiagramLsb = 1

	LineSequence = 1
	LineRatio = 1
	LineLabel = 3
	LineText = 0

	TrigramSequence = 1
	TrigramRatio = 0
	TrigramLabel = 2
	TrigramText = 0

	HexagramSequence = 2
	HexagramRatio = 0
	HexagramLabel = 9
	HexagramText = 1 # 6

	#Random m_ranSession = new Random(DateTime.Now.Millisecond)

	@staticmethod
	def Initialise():
		#SetLSB(DiagramLsb == 1)
		pass

	strDiagramSettings = [
		["Diagram Mode", "Explore", "Animate", "Touch Cast", "Mind Cast", "Auto Cast", "", "", "", "", "", "", "", ""],
		["Diagram Type", "Hexagram", "Trigram", "Line", "", "", "", "", "", "", "", "", "", ""],
		["Diagram Color", "Mono", "Dual", "Trigram", "Rgb", "", "", "", "", "", "", "", "", ""],
		["Diagram Speed", "Slow", "Medium", "Fast", "", "", "", "", "", "", "", "", "", ""],
		["Diagram LSB", "Top", "Bottom", "", "", "", "", "", "", "", "", "", "", ""],

		["Line Sequence", "Numeric", "Natural", "", "", "", "", "", "", "", "", "", "", ""],
		["Line Ratio", "Equal", "Coin", "Yarrow", "Marbles", "Yin", "Yang", "", "", "", "", "", "", ""],
		["Line Label", "Numeric", "Alpha", "Coin", "Line", "Season", "Direction", "Moon", "Tide", "", "", "", "", ""],
		["Line Text", "Wikipedia", "", "", "", "", "", "", "", "", "", "", "", ""],

		["Trigram Sequence", "Numeric", "Primal", "Inner", "Direction", "", "", "", "", "", "", "", "", ""],
		["Trigram Ratio", "Equal", "", "", "", "", "", "", "", "", "", "", "", ""],
		["Trigram Label", "Numeric", "Alpha", "Trigram", "Class", "Direction", "Family", "Animal", "Body", "", "", "", "", ""],
		["Trigram Text", "Wikipedia", "", "", "", "", "", "", "", "", "", "", "", ""],

		["Hexagram Sequence", "Numeric", "Fuxi", "Wen", "Mystery", "8 Palaces", "Mawangdui", "Yang Lian", "Getojack", "Lattice Path", "Sovereign", "Double", "Same Inverse", "Opposite & Inverse"],
		["Hexagram Ratio", "Equal", "", "", "", "", "", "", "", "", "", "", "", ""],
		["Hexagram Label", "Numeric", "Alpha", "Hatcher", "Heyboer", "Karcher", "Legge", "Machovec", "Marshall", "Rutt", "Vivash", "Wilhelm", "", ""],

		#["Hexagram Text", "Andrade", "Chinese", "Legge", "Hatcher", "Heyboer", "Regis", "Wilhelm", "YellowBridge", "", "", "", "", ""],
		
		["Hexagram Text", "Legge", "Wilhelm", "", "", "", "", "", "", "", "", "", "", ""],
	]

	nBitSequences = [
		[0,1],
		[1,0]
	]

	nBitRatios = [
		[1,1]
	]

	strBitLabels = [
		["0", "1"],
		["Zero", "One"],
		["Yin", "Yang"]
	]

	nLineSequences = [
		[0,1,2,3],
		[2,3,1,0]
	]

	nLineRatios = [
		[1,1,1,1],
		[1,3,3,1],
		[1,5,7,3],
		[1,4,4,1],
		[5,1,10,2],
		[2,10,1,5],
	]

	strLineLabels = [
		["0", "1", "2", "3"],
		["Zero", "One", "Two", "Three"],
		["Three Heads", "One Tail", "One Head", "Three Tails"],
		["Old Yin", "Young Yang", "Young Yin", "Old Yang"],
		["Winter", "Spring", "Autumn", "Summer"],
		["South", "West", "East", "North"],
		["New Moon", "Waxing", "Waning", "Full Moon"],
		["Low Tide", "Coming In", "Going Out", "High Tide"]
	]

	nTrigramSequences = [
		[0,1,2,3,4,5,6,7],
		[4,5,2,7,3,6,1,0],
		[1,6,4,2,5,0,7,3],
		[4,1,6,7,3,2,5,0]
	]

	nTrigramRatios = [
		[1,1,1,1,1,1,1,1]
	]

	strTrigramLabels = [
		["0", "1", "2", "3", "4", "5", "6", "7"],
		["Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven"],
		["Earth", "Thunder", "Water", "Lake", "Mountain", "Fire", "Wind", "Heaven"],
		["Weak", "Movement", "Danger", "Pleasure", "Rest", "Dependence", "Penetrating", "Strong"],
		["South", "North East", "West", "North West", "South East", "East", "South West", "North"],
		["Mother", "Eldest Son", "Middle Son", "Youngest Daughter", "Youngest Son", "Middle Daughter", "Eldest Daughter", "Father"],
		["Cow", "Dragon", "Pig", "Sheep", "Dog", "Phesant", "Cock", "Horse"],
		["Belly", "Foot", "Ear", "Mouth", "Hand", "Eye", "Thigh", "Head"]
	]

	nHexagramSequences = [
		[0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63],
		[0,32,16,48,8,40,24,56,4,36,20,52,12,44,28,60,2,34,18,50,10,42,26,58,6,38,22,54,14,46,30,62,1,33,17,49,9,41,25,57,5,37,21,53,13,45,29,61,3,35,19,51,11,43,27,59,7,39,23,55,15,47,31,63],
		[1,23,6,18,14,35,45,10,15,50,39,53,61,54,31,33,7,2,28,59,38,62,47,4,44,16,46,57,30,48,27,42,22,26,3,40,51,21,17,25,34,20,63,37,55,29,49,13,19,41,58,60,52,36,56,8,11,24,5,9,32,12,43,0],
		[63,57,58,44,59,46,51,25,60,48,56,28,52,33,36,11,61,50,54,31,55,40,39,15,53,35,38,18,37,20,21,6,62,43,45,24,47,27,32,10,49,30,41,14,34,17,19,5,42,23,26,9,29,13,16,4,22,8,12,3,7,2,1,0],
		[32,33,23,34,61,22,12,35,9,8,10,63,62,21,11,36,39,18,16,17,60,19,13,38,58,15,57,56,59,20,14,37,5,46,52,27,24,25,47,26,6,45,51,28,49,48,50,7,4,43,53,30,31,42,40,41,3,44,54,29,2,55,1,0],
		[32,38,36,35,34,37,39,33,26,24,29,28,27,30,31,25,18,22,16,20,19,21,23,17,42,46,44,40,43,45,47,41,10,14,12,11,8,13,15,9,50,54,53,52,51,48,55,49,58,63,61,60,59,62,56,57,1,6,4,3,2,5,7,0],
		[17,20,35,3,51,50,33,2,18,22,36,5,52,53,38,4,26,24,40,9,56,57,42,8,31,28,47,15,63,62,45,14,30,27,46,13,60,61,44,12,29,25,41,11,59,58,43,10,21,23,37,7,55,54,39,6,16,19,34,1,48,49,32,0],
		[7,35,21,49,14,42,28,56,39,3,53,17,46,10,60,24,23,51,5,33,30,58,12,40,55,19,37,1,62,26,44,8,15,43,29,57,6,34,20,48,47,11,61,25,38,2,52,16,31,59,13,41,22,50,4,32,63,27,45,9,54,18,36,0],
		[0,1,3,2,7,6,4,5,15,14,12,13,8,9,11,10,31,30,28,29,24,25,27,26,16,17,19,18,23,22,20,21,63,62,60,61,56,57,59,58,48,49,51,50,55,54,52,53,32,33,35,34,39,38,36,37,47,46,44,45,40,41,43,42],
		[6,7,-1,8,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,11,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,4,-1,-1,-1,-1,-1,-1,-1,3,-1,-1,-1,2,-1,1,0],
		[0,-1,-1,-1,-1,-1,-1,-1,-1,1,-1,-1,-1,-1,-1,-1,-1,-1,2,-1,-1,-1,-1,-1,-1,-1,-1,3,-1,-1,-1,-1,-1,-1,-1,-1,4,-1,-1,-1,-1,-1,-1,-1,-1,5,-1,-1,-1,-1,-1,-1,-1,-1,6,-1,-1,-1,-1,-1,-1,-1,-1,7],
		[0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,1,-1,-1,-1,-1,-1,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,3,-1,-1,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,5,-1,-1,-1,-1,-1,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7],
		[-1,-1,-1,-1,-1,-1,-1,0,-1,-1,-1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,2,-1,-1,-1,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,4,-1,-1,-1,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,6,-1,-1,-1,7,-1,-1,-1,-1,-1,-1,-1]
	]

	nHexagramRatios = [
		[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1]
	]

	strHexagramLabels = [
		["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63"],
		["Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fiveteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty", "Twenty One", "Twenty Two", "Twenty Three", "Twenty Four", "Twenty Five", "Twenty Six", "Twenty Seven", "Twenty Eight", "Twenty Nine", "Thirty", "Thirty One", "Thirty Two", "Thirty Three", "Thirty Four", "Thirty Five", "Thirty Six", "Thirty Seven", "Thirty Eight", "Thirty Nine", "Forty", "Forty One", "Forty Two", "Forty Three", "Forty Four", "Forty Five", "Forty Six", "Forty Seven", "Forty Eight", "Forty Nine", "Fifty", "Fifty One", "Fifty Two", "Fifty Three", "Fifty Four", "Fifty Five", "Fifty Six", "Fifty Seven", "Fifty Eight", "Fifty Nine", "Sixty", "Sixty One", "Sixty Two", "Sixty Three"],
		["Accepting", "Returning", "The Militia", "Taking Charge", "Authenticity", "Brightness Obscured", "Advancement", "Interplay", "Readiness", "Arousal", "Release", "Little Sister's Marriage", "Smallness in Excess", "Abundance", "Continuity", "Big and Strong", "Belonging", "Rallying", "Exposure", "Boundaries", "Impasse", "Already Complete", "The Well", "Anticipation", "Collectedness", "Following", "Exhaustion", "Satisfaction", "Reciprocity", "Seasonal Change", "Greatness in Excess", "Decisiveness", "Decomposing", "Hungry Mouth", "Inexperience", "Decreasing", "Stillness", "Adornment", "Detoxifying", "Raising Great Beasts", "Expansion", "Biting Through", "Not Yet Complete", "Estrangement", "The Wanderer", "Arising", "The Cauldron", "Big Domain", "Perspective", "Increasing", "Scattering", "The Truth Within", "Gradual Progress", "Family Members", "Adaptation", "Raising Small Beasts", "Separating", "Without Pretense", "Contention", "Respectful Conduct", "Distancing", "Fellowship With Others", "Dissipation", "Creating",],
		["The Energy of Earth", "Return to your Town", "Legion, Leading", "Overseeing", "Give and Take", "Brightness and Leveling", "Step by Step", "Mount Tai", "Weaving Images", "Thunderbolt", "Take the Horns", "Marrying Maiden", "Across the Small Pass", "Drums of Victory", "Steady the Helm of the Heart", "A Man of Stone", "Stand By", "The Spark of Life", "The Teachings of Danger", "The Measure", "Cold Feet", "Already Across", "The Well", "Waiting for a Break in the Weather", "Gathering", "Follow without Resistance", "Enclosed Tree", "Opening", "Affect and Affection", "Skinning, Revolution", "Across the Great Pass", "The Speaking Staff", "The Wine Skin and the Knife", "Jaws", "Not Knowing", "The Empty Cauldron", "Resistance", "Flower Power", "Can o' Worms", "Raising Big Cattle", "The Gift", "Biting Through", "Not Yet Across", "Looking Askance", "Itinerant Troops", "To Catch the Bird of Brightness", "The Ritual Cauldron", "Great Assets", "The Heron", "The Bowl of the Raingod", "The Flood", "Inner Sincerity", "The Waterwheel", "Family of Man", "The Seal", "Tending Small Livestock", "To Say No", "Natural", "The Gong Speaks", "The Footprints of the Ancestors", "Save your Bacon", "Mankind", "Heir", "The Directing Power of Heaven",],
		["Field, Yielding", "Returning", "Legions", "Nearing", "Humbling, Holding Back", "Hiding Brightness, Brightness Hiding", "Ascending", "Pervading", "Providing For, Responding", "Shake, Arousing", "Loosening, Deliverance", "Converting the Maiden", "Small Exceeding", "Abounding", "Persevering", "Great Invigorating", "Grouping", "Sprouting", "Repeating the Gorge, Venturing", "Articulating", "Limping, Difficulties", "Already Fording", "The Well", "Attending", "Clustering", "Following", "Confining, Oppressed", "Open, Expressing", "Conjoining", "Skinning", "Great Exceeding", "Deciding, Resolution, Parting", "Stripping", "Jaws, Swallowing", "Enveloping", "Diminishing", "Bound, Stabilizing", "Adorning", "Corruption, Renovating", "Great Accumulating", "Prospering", "Gnawing and Bitting Through", "Not Yet Fording", "Diverging, Polarizing", "Sojourning, Quest", "Radiance, Clarifying", "The Vessel, Holding", "Great Possessions, Great Possessing", "Viewing", "Augmenting", "Dispersing", "Connecting to Center, Centring Accord", "Infiltrating, Gradually Advancing", "Dwelling (Clan) People", "Gently Penetrating, Ground, Penetrating", "Small Accumulating", "Obstruction", "Without Embroiling", "Arguing", "Treading", "Retiring", "Concording People", "Coupling, Welcoming", "Force, Persisting",],
		["Subordination, Docility", "Returning, Coming Back", "The Host, The Army", "The Approach of Authority", "Humility, Merit", "Intelligence Wounded or Repressed", "Advancing and Ascending", "Waxing", "Pleasure, Harmony, Satisfaction", "Conduct in a Time of Movement", "Loosing, Unravelling a Complication", "The Marrying Away of a Younger Sister", "Exceeding in what is Small", "Abundant Prosperity", "Perseverance, Continuously Maintaining", "Abundance of Strength and Vigor", "Union, Attachment", "Distressed, Struggle", "Dangerous Defile", "Regulating and Restraining", "Difficulties, Incompetency", "Successful Accomplishment", "Moralisings on a Well, Lessons from a Well", "Waiting", "Collecting Together", "Following", "Straitened and Distressed", "Pleasure, Complacent Satisfaction", "Moving, Influencing to Movement", "Changing, Changes", "Extraordinary, Exceeding", "Resolute Removal, Determination", "Falling, Causing to Fall, Decay, Overthrow", "Jaws Hang Down, Nourishing", "Ignorance, Without Experience", "Diminution, Contributing from one's Own", "Resting and Arresting, Keeping at Rest", "Ornamental, Adorning", "Having Painful/Troublesome Services to Do", "Great Restraand Accumulation", "Advancing", "Union by Gnawing, Biting Through", "Accomplishment Not Yet Realized", "Division, Mutual Alienation, Disunion", "Traveling Stranger", "Double Brightness", "The Cauldron", "Great Havings", "Manifesting and Contemplating", "Adding To, Gifts Received", "Dissipation, Dispersion", "Inmost Sincerity", "Gradually Progressing, Gradually Advancing", "Family Members, Household", "Flexibility and Penetration", "Nourishing", "Waning, Distress", "Free from Insincerity", "Contention, Strife", "Treading, Walk Softly", "Retiring, Seclusion", "Union of Men, Brotherhood", "Insinuation, Resisting Encroachment", "Active, Vigilant, Heaven",],
		["Loving Service of the Earth Mother", "Minor Setback", "Group Leadership", "Moving Ahead", "Humility", "Being Misunderstood", "Onward and Upward", "Personal Development", "Happiness", "The Web of Anxiety", "Liberation", "Passive Progress", "Timing", "Using and Abusing", "Maturity", "Optimal Influence", "Unifying Spirit", "Growth", "Shorcomings Increase Problems", "Rules", "Overcoming Obstacles", "Complacency", "First Things", "Watchful Waiting", "Unifying Spirit", "Associating with Others", "Out of Darkness", "Balance and Pleasure", "Influencing Others", "Timely Change", "Continuing Problems", "Remain Steadfast", "Dealing with Deceit", "Self-Sufficiency", "Learning", "Selflessness", "Stop, Look, Listen", "The Artificial You", "Overcoming Backwardness", "Self-Control", "Succeeding", "Accepting Punishment", "Great Problems, Great Plans", "Alienation", "Reckless Ambition", "The golden Mean", "High Ideas", "Real Wealth", "Perception", "Inspiration", "Human Truth", "Be Yourself", "Continued Growth", "The Family", "Search Out Evil", "Moderation", "Frustration", "Childlike Simplicity", "Challenge, Conflict", "Tact", "Strategic Withdrawal", "Friendship", "Negative Influences", "Strength of the Dragon",],
		["The Receptive", "Return", "The Army", "Growth", "Modesty", "Fading Light", "Growth", "Peace", "Enthusiasm", "The Arousing", "Liberation", "The Maiden", "Trapped Power", "Abundance", "Duration", "Great Power", "Union", "Difficult Start", "The Abyss", "Limitation", "Obstruction", "Completion", "The Well", "Waiting", "Gathering", "Following", "Oppression", "Joy", "Relating", "Revolution", "Great Weight", "Resolution", "Disintegration", "Provision", "Youthful Folly", "Decrease", "Keeping Still", "Gracefulness", "Repairing Decay", "Restraint", "Progress", "Biting Through", "Arrival", "Opposition", "The Wanderer", "Fire", "The Cauldron", "Plenty of Time", "View", "Increase", "Dispersion", "Truth", "Development", "The Family", "Gentle Wind", "Restraining", "Disharmony", "Innocence", "Conflict", "Being Cautious", "Retreat", "Fellowship", "Temptation", "The Creative",],
		["Earth", "Returning", "Troops", "Keening", "Rat", "Crying Pheasant", "Going Up", "Great", "Elephant", "Thunder", "Unloosing", "Marriage", "Passing, Minor", "Thick", "Fixing", "Big Injury", "Joining", "Massed", "Pit", "Juncture", "Stumbling", "Already Across", "Well", "Waiting", "Together", "Pursuit", "Beset", "Satisfaction", "Chopping", "Leather", "Passing, Major", "Skipping", "Flaying", "Molars", "Dodder", "Diminishing", "Cleaving", "Bedight", "Mildew", "Farming, Major", "Advancing", "Flaying", "Not Yet Across", "Espy", "Sojourner", "Oriole", "Tripod Bowl", "Large, There", "Observing", "Enriching", "Gushing", "Trying Captives", "Settling", "Household", "Food Offerings", "Farming Minor", "Bad", "Unexpected", "Dispute", "Stepping", "Pig, Piglet", "Mustering", "Locking", "Active",],
		["Receptive", "Return", "Organisation", "Approach", "Modesty", "Delusion", "Ascend", "Peace", "Passion", "Arousing", "Release", "Propriety", "Surplus", "Abundance", "Duration", "Power", "Union", "Difficulty", "Danger", "Limitation", "Obstruction", "Rest", "Reserve", "Waiting", "Assemble", "Following", "Exhaustion", "Pleasure", "Influence", "Revolution", "Excess", "Breakthrough", "Collapse", "Nourishment", "Inexperience", "Decrease", "Meditation", "Grace", "Repair", "Discipline", "Progress", "Resolution", "Strain", "Opposition", "Discovery", "Clarity", "Culture", "Possession", "Contemplation", "Increase", "Dispersion", "Sincerity", "Development", "Order", "Faith", "Restraint", "Stagnation", "Innocence", "Conflict", "Conduct", "Retreat", "Friendship", "Encounter", "Creative"],
		["The Receptive", "Return (The Turning Point)", "The Army", "Approach", "Modesty", "Darkening of the Light", "Pushing Upward", "Peace", "Enthusiasm", "The Arousing (Shock, Thunder)", "Deliverance", "The Marrying Maiden", "Preponderance of the Small", "Abundance (Fullness)", "Duration", "The Power of the Great", "Holding Together (Union)", "Difficulty at the Beginning", "The Abysmal (Water)", "Limitation", "Obstruction", "After Completion", "The Well", "Waiting (Nourishment)", "Gathering Together (Massing)", "Following", "Oppression (Exhaustion)", "The Joyous, Lake", "Influence (Wooing)", "Revolution (Molting)", "Preponderance of the Great", "Break-through (Resoluteness)", "Splitting Apart", "Corners of the Mouth, Prov. Nourishment", "Youthful Folly", "Decrease", "Keeping Still, Mountain", "Grace", "Work on What Has Been Spoiled (Decay)", "The Taming Power of the Great", "Progress", "Biting Through", "Before Completion", "Opposition", "The Wanderer", "The Clinging (Fire)", "The Cauldron", "Possession in Great Measure", "Contemplation (View)", "Increase", "Dispersion (Dissolution)", "Inner Truth", "Development (Gradual Progress)", "The Family (The Clan)", "The Gentle (The Penetrating, Wind)", "The Taming Power of the Small", "Standstill (Stagnation)", "Innocence (The Unexpected)", "Conflict", "Treading (Conduct)", "Retreat", "Fellowship With Men", "Coming to Meet", "The Creative",],
	]

	strSymbols = [
		"坤", "復", "師", "臨", "謙", "明夷", "升", "泰", "豫", "震", "解", "歸妹", "小過", "豐", "恆", "大壯", "比", "屯", "坎", "節", "蹇", "既濟", "井", "需", "萃", "隨", "困", "兌", "咸", "革", "大過", "夬", "剝", "頤", "蒙", "損", "艮", "賁", "蠱", "大畜", "晉", "噬嗑", "未濟", "睽", "旅", "離", "鼎", "大有", "觀", "益", "渙", "中孚", "漸", "家人", "巽", "小畜", "否", "無妄", "訟", "履", "遯", "同人", "姤", "乾",
	]

	nTrigramLsbs = [
		[0,4,2,6,1,5,3,7],
		[0,1,2,3,4,5,6,7],
	]

	strTrigramLsbs = [
		["0", "4", "2", "6", "1", "5", "3", "7"],
		["Zero", "Four", "Two", "Six", "One", "Five", "Three", "Seven"],
		["0", "1", "2", "3", "4", "5", "6", "7"],
		["Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven"],
	]

	nHexagramLsbs = [
		[0,32,16,48,8,40,24,56,4,36,20,52,12,44,28,60,2,34,18,50,10,42,26,58,6,38,22,54,14,46,30,62,1,33,17,49,9,41,25,57,5,37,21,53,13,45,29,61,3,35,19,51,11,43,27,59,7,39,23,55,15,47,31,63],
		[0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63],
	]

	strHexagramLsbs = [
		["0", "32", "16", "48", "8", "40", "24", "56", "4", "36", "20", "52", "12", "44", "28", "60", "2", "34", "18", "50", "10", "42", "26", "58", "6", "38", "22", "54", "14", "46", "30", "62", "1", "33", "17", "49", "9", "41", "25", "57", "5", "37", "21", "53", "13", "45", "29", "61", "3", "35", "19", "51", "11", "43", "27", "59", "7", "39", "23", "55", "15", "47", "31", "63"],
		["Zero", "Thirty Two", "Sixteen", "Forty Eight", "Eight", "Forty", "Twenty Four", "Fifty Six", "Four", "Thirty Six", "Twenty", "Fifty Two", "Twelve", "Forty Four", "Twenty Eight", "Sixty", "Two", "Thirty Four", "Eighteen", "Fifty", "Ten", "Forty Two", "Twenty Six", "Fifty Eight", "Six", "Thirty Eight", "Twenty Two", "Fifty Four", "Fourteen", "Forty Six", "Thirty", "Sixty Two", "One", "Thirty Three", "Seventeen", "Forty Nine", "Nine", "Forty One", "Twenty Five", "Fifty Seven", "Five", "Thirty Seven", "Twenty One", "Fifty Three", "Thirteen", "Forty Five", "Twenty Nine", "Sixty One", "Three", "Thirty Five", "Nineteen", "Fifty One", "Eleven", "Forty Three", "Twenty Seven", "Fifty Nine", "Seven", "Thirty Nine", "Twenty Three", "Fifty Five", "Fiveteen", "Forty Seven", "Thirty One", "Sixty Three"],
		["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63"],
		["Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fiveteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty", "Twenty One", "Twenty Two", "Twenty Three", "Twenty Four", "Twenty Five", "Twenty Six", "Twenty Seven", "Twenty Eight", "Twenty Nine", "Thirty", "Thirty One", "Thirty Two", "Thirty Three", "Thirty Four", "Thirty Five", "Thirty Six", "Thirty Seven", "Thirty Eight", "Thirty Nine", "Forty", "Forty One", "Forty Two", "Forty Three", "Forty Four", "Forty Five", "Forty Six", "Forty Seven", "Forty Eight", "Forty Nine", "Fifty", "Fifty One", "Fifty Two", "Fifty Three", "Fifty Four", "Fifty Five", "Fifty Six", "Fifty Seven", "Fifty Eight", "Fifty Nine", "Sixty", "Sixty One", "Sixty Two", "Sixty Three"],
	]

	nHatcherPage = [
		67, 74, 81, 87, 93, 99, 105, 111, 117, 123,
		129, 135, 141, 147, 153, 159, 165, 171, 177, 183,
		189, 195, 201, 207, 213, 219, 225, 231, 237, 243,
		249, 255, 261, 267, 273, 279, 285, 291, 297, 303,
		309, 315, 321, 327, 333, 339, 245, 351, 357, 363,
		369, 375, 381, 387, 393, 399, 405, 411, 417, 423,
		429, 435, 441, 447
	]

	"""
	def SetLSB(bool bTop):
	{
		if (bTop)
		{
			for (int i = 0; i < 8; ++i)
			{
				nTrigramSequences[0, i] = nTrigramLsbs[0, i];
				strTrigramLabels[0, i] = strTrigramLsbs[0, i];
				strTrigramLabels[1, i] = strTrigramLsbs[1, i];
			}
			for (int i = 0; i < 64; ++i)
			{
				nHexagramSequences[0, i] = nHexagramLsbs[0, i];
				strHexagramLabels[0, i] = strHexagramLsbs[0, i];
				strHexagramLabels[1, i] = strHexagramLsbs[1, i];
			}
		}
		else
		{
			for (int i = 0; i < 8; ++i)
			{
				nTrigramSequences[0, i] = nTrigramLsbs[1, i];
				strTrigramLabels[0, i] = strTrigramLsbs[2, i];
				strTrigramLabels[1, i] = strTrigramLsbs[3, i];
			}
			for (int i = 0; i < 64; ++i)
			{
				nHexagramSequences[0, i] = nHexagramLsbs[1, i];
				strHexagramLabels[0, i] = strHexagramLsbs[2, i];
				strHexagramLabels[1, i] = strHexagramLsbs[3, i];
			}
		}
	}
	"""

	def DiagramSetting(self, nIndex, nValue):
		return self.strDiagramSettings[nIndex, nValue + 1]

	def DiagramSetting(self, strName, nValue):
		for i in range(17):
			if self.strDiagramSettings[i, 0] == strName:
				return self.DiagramSetting(i, nValue)
		return ""

	def DiagramSetting(self, nIndex, strName):
		for i in range(17):
			if self.strDiagramSettings[nIndex, i] == strName:
				return i - 1
		return 0
