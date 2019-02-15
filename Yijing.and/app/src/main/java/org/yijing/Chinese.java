package org.yijing;

public class Chinese
{

	public static String[] strText = {
		"坤 元亨利牝馬之貞。君子有攸往。先迷後得。主利。西南得朋。東北喪朋。安貞吉。</br></br>初六 履霜堅冰至。</br></br>六二 直方大。不習无不利。</br></br>六三 含章可貞。或從王事。无成有終。</br></br>六四 括囊。无咎无譽。</br></br>六五 黃裳。元吉。</br></br>上六 龍戰于野。其血玄黃。</br></br>用六 利永貞。</br></br>",
		"復 亨。出入无疾。朋來无咎。反復其道。七日來復。利有攸往。</br></br>初九 不遠復。无祗悔。元吉。</br></br>六二 休復。吉。</br></br>六三 頻復。厲。无咎。</br></br>六四 中行獨復。</br></br>六五 敦復。无悔。</br></br>上六 迷復。凶。有災眚。用行師。終有大敗。 以其國君凶。至于十年不克征。</br></br>",
		"師 貞。丈人吉。无咎。</br></br>初六 師出以律。否臧凶。</br></br>九二 在師中吉。无咎。王三錫命。</br></br>六三 師或輿尸。凶。</br></br>六四 師左次。无咎。</br></br>六五 田有禽。利執言。无咎。長子帥師。弟子輿尸。貞凶。</br></br>上六 大君有命。開國承家。小人勿用。</br></br>",
		"臨 元亨利貞。至于八月有凶。</br></br>初九 咸臨貞吉。</br></br>九二 咸臨吉。无不利。</br></br>六三 甘臨。无攸利。既憂之。无咎。</br></br>六四 至臨。无咎。</br></br>六五 知臨。大君之宜。吉。</br></br>上六 敦臨。吉。无咎。</br></br>",
		"謙 亨。君子有終。</br></br>初六 謙謙君子。用涉大川。吉。</br></br>六二 鳴謙。貞吉。</br></br>九三 勞謙君子。有終吉。</br></br>六四 无不利撝謙。</br></br>六五 不富以其鄰。利用侵伐。无不利。</br></br>上六 鳴謙。利用行師。征邑國。</br></br>",
		"明夷 利艱貞。</br></br>初九 明夷于飛。垂其翼。君子于行。三日不食。有攸往。主人有言。</br></br>六二 明夷。夷于左股。用拯馬壯吉。</br></br>九三 明夷于南狩。得其大首。不可疾貞。</br></br>六四 入于左腹。獲明夷之心。于出門庭。</br></br>六五 箕子之明夷。利貞。</br></br>上六 不明晦。初登于天。後入于地。</br></br>",
		"升 元亨。用見大人。勿恤。南征吉。</br></br>初六 允升大吉。</br></br>九二 孚乃利用禴。无咎。</br></br>九三 升虛邑。</br></br>六四 王用亨于岐山。吉。无咎。</br></br>六五 貞吉升階。</br></br>上六 冥升。利于不息之貞。</br></br>",
		"泰 小往大來。吉亨。</br></br>初九 拔茅茹。以其彙。征吉。</br></br>九二 包荒。用馮河。不遐遺。朋亡。得尚于中行。</br></br>九三 无平不陂。无往不復。艱貞无咎。勿恤其孚。于食有福。</br></br>六四 翩翩。不富以其鄰。不戒以孚。</br></br>六五 帝乙歸妹。以祉元吉。</br></br>上六 城復于隍。勿用師。自邑告命。貞吝。</br></br>",
		"豫 利建侯行師。</br></br>初六 鳴豫。凶。</br></br>六二 介于石。不終日。貞吉。</br></br>六三 盱豫悔。遲有悔。</br></br>九四 由豫。大有得。勿疑。朋盍簪。</br></br>六五 貞疾。恆不死。</br></br>上六 冥豫。成有渝。无咎。</br></br>",
		"震 亨。震來虩虩。笑言啞啞。震驚百里。不喪匕鬯。</br></br>初九 震來虩虩。後笑言啞啞。吉。</br></br>六二 震來厲。億喪貝。躋于九陵。勿逐。七日得。</br></br>六三 震蘇蘇。震行无眚。</br></br>九四 震遂泥。</br></br>六五 震往來厲。意无喪有事。</br></br>上六 震索索。視矍矍。征凶。震不于其躬。于其鄰。无咎。婚媾有言。</br></br>",
		"解 利西南。无所往。其來復吉。有攸往。夙吉。</br></br>初六 无咎。</br></br>九二 田獲三狐。得黃矢。貞吉。</br></br>六三 負且乘。致寇至。貞吝。</br></br>九四 解而拇。朋至斯孚。</br></br>六五 君子維有解。吉。有孚于小人。</br></br>上六 公用射隼于高墉之上。獲之无不利。</br></br>",
		"歸妹 征凶。无攸利。</br></br>初九 歸妹以娣。跛能履。征吉。</br></br>九二 眇能視。利幽人之貞。</br></br>六三 歸妹以須。反歸以娣。</br></br>九四 歸妹愆期。遲歸有時。</br></br>六五 帝乙歸妹。其君之袂。不如其娣之袂良。月幾望吉。</br></br>上六 女承筐无實。士刲羊无血。无攸利。</br></br>",
		"小過 亨。利貞。可小事。不可大事。飛鳥遺之音。不宜上宜下。大吉。</br></br>初六 飛鳥以凶。</br></br>六二 過其祖。遇其妣。不及其君。遇其臣。无咎。</br></br>九三 弗過防之。從或戕之。凶。</br></br>九四 无咎。弗過遇之。往厲必戒。勿用永貞。</br></br>六五 密雲不雨。自我西郊。公弋取彼在穴。</br></br>上六 弗遇過之。飛鳥離之。凶。是謂災眚。</br></br>",
		"豐 亨。王假之。勿憂。宜日中。</br></br>初九 遇其配主。雖旬无咎。往有尚。</br></br>六二 豐其蔀。日中見斗。往得疑疾。有孚發若。吉。</br></br>九三 豐其沛。日中見沬。折其右肱。无咎。</br></br>九四 豐其蔀。日中見斗。遇其夷主。吉。</br></br>六五 來章。有慶譽吉。</br></br>上六 豐其屋。蔀其家。闚其戶。闃其无人。三歲不覿。凶。</br></br>",
		"恆 亨。无咎。利貞。利有攸往。</br></br>初六 浚恆貞凶。无攸利。</br></br>九二 悔亡。</br></br>九三 不恆其德。或承之羞。貞吝。</br></br>九四 田无禽。</br></br>六五 恒其德貞。婦人吉。夫子凶。</br></br>上六 振恆凶。</br></br>",
		"大壯 利貞。</br></br>初九 壯于趾。征凶有孚。</br></br>九二 貞吉。</br></br>九三 小人用壯。君子用罔。貞厲。羝羊觸藩。羸其角。</br></br>九四 貞吉。悔亡。藩決不羸。壯于大輿之輹。</br></br>六五 喪羊于易。无悔。</br></br>上六 羝羊觸藩。不能退。不能遂。无攸利。艱則吉。</br></br>",
		"比 吉。原筮元永貞。无咎。不寧方來。後夫凶。</br></br>初六 有孚比之。无咎。有孚盈缶。終來有它吉。</br></br>六二 比之自內。貞吉。</br></br>六三 比之匪人。</br></br>六四 外比之。貞吉。</br></br>九五 顯比。王用三驅。失前禽。邑人不誡。吉。</br></br>上六 比之无首。凶。</br></br>",
		"屯 元亨利貞。勿用有攸往。利建侯。</br></br>初九 磐桓。利居貞。利建侯。</br></br>六二 屯如邅如。乘馬班如。匪寇婚媾。女子貞不字。十年乃字。</br></br>六三 即鹿無虞。惟入于林中。君子幾不如舍。往吝。</br></br>六四 乘馬班如。求婚媾。往吉。无不利。</br></br>九五 屯其膏。小貞吉。大貞凶。</br></br>上六 乘馬班如。泣血漣如。</br></br>",
		"習坎。有孚。維心亨。行有尚。</br></br>初六 習坎。入于坎窞。凶。</br></br>九二 坎有險。求小得。</br></br>六三 來之坎坎。險且枕。入于坎窞。勿用。</br></br>六四 樽酒簋貳。用缶。納約自牖。終无咎。</br></br>九五 坎不盈。祗既平。无咎。</br></br>上六 係用徽纆。寘于叢棘。三歲不得。凶。</br></br>",
		"節 亨。苦節不可貞。</br></br>初九 不出戶庭。无咎。</br></br>九二 不出門庭。凶。</br></br>六三 不節若。則嗟若。无咎。</br></br>六四 安節亨。</br></br>九五 甘節吉。往有尚。</br></br>上六 苦節貞凶。悔亡。</br></br>",
		"蹇 利西南。不利東北。利見大人。貞吉。</br></br>初六 往蹇來譽。</br></br>六二 王臣蹇蹇。匪躬之故。</br></br>九三 往蹇來反。</br></br>六四 往蹇來連。</br></br>九五 大蹇朋來。</br></br>上六 往蹇來碩。吉。利見大人。</br></br>",
		"既濟 亨小。利貞。初吉。終亂。</br></br>初九 曳其輪。濡其尾。无咎。</br></br>六二 婦喪其茀。勿逐。七日得。</br></br>九三 高宗伐鬼方。三年克之。小人勿用。</br></br>六四 繻有衣袽。終日戒。</br></br>九五 東鄰殺牛。不如西鄰之禴祭。實受其福。</br></br>上六 濡其首。厲。</br></br>",
		"井 改邑不改井。无喪无得。往來井井。汔至亦未繘井。羸其瓶。凶。</br></br>初六 井泥不食。舊井无禽。</br></br>九二 井谷射鮒。甕敝漏。</br></br>九三 井渫不食。為我心惻。可用汲。王明。並受其福。</br></br>六四 井甃无咎。</br></br>九五 井冽。寒泉食。</br></br>上六 井收勿幕。有孚元吉。</br></br>",
		"需 有孚。光亨貞吉。利涉大川。</br></br>初九 需于郊。利用恆。无咎。</br></br>九二 需于沙。小有言。終吉。</br></br>九三 需于泥。致寇至。</br></br>六四 需于血。出自穴。</br></br>九五 需于酒食。貞吉。</br></br>上六 入于穴。有不速之客三人來。敬之終吉。</br></br>",
		"萃 亨。王假有廟。利見大人。亨。利貞。用大牲吉。利有攸往。</br></br>初六 有孚不終。乃亂乃萃。若號一握為笑。勿恤。往无咎。</br></br>六二 引吉无咎。孚乃利用禴。</br></br>六三 萃如嗟如。无攸利。往无咎。小吝。</br></br>九四 大吉无咎。</br></br>九五 萃有位。无咎匪孚。元永貞。悔亡。</br></br>上六 齎咨涕洟。无咎。</br></br>",
		"隨 元亨利貞。无咎。</br></br>初九 官有渝。貞吉。出門交有功。</br></br>六二 係小子。失丈夫。</br></br>六三 係丈夫。失小子。隨有求得。利居貞。</br></br>九四 隨有獲。貞凶。有孚在道以明。何咎。</br></br>九五 孚于嘉。吉。</br></br>上六 拘係之。乃從維之。王用亨于西山。</br></br>",
		"困 亨。貞大人吉。无咎。有言不信。</br></br>初六 臀困于株木。入于幽谷。三歲不覿。</br></br>九二 困于酒食。朱紱方來。利用享祀。征凶无咎。</br></br>六三 困于石。據于蒺蔾。入于其宮。不見其妻。凶。</br></br>九四 來徐徐。困于金車。吝。有終。</br></br>九五 劓刖。困于赤紱。乃徐有說。利用祭祀。</br></br>上六 困于葛藟。于臲卼。曰動悔有悔。征吉。</br></br>",
		"兌 亨。利貞。</br></br>初九 和兌吉。</br></br>九二 孚兌吉。悔亡。</br></br>六三 來兌凶。</br></br>九四 商兌未寧。介疾有喜。</br></br>九五 孚于剝。有厲。</br></br>上六 引兌。</br></br>",
		"咸 亨。利貞。取女吉。</br></br>初六 咸其拇。</br></br>六二 咸其腓。凶。居吉。</br></br>九三 咸其股。執其隨。往吝。</br></br>九四 貞吉悔亡。憧憧往來。朋從爾思。</br></br>九五 咸其脢。无悔。</br></br>上六 咸其輔頰舌。</br></br>",
		"革 已日乃孚。元亨。利貞。悔亡。</br></br>初九 鞏用黃牛之革。</br></br>六二 已日乃革之。征吉无咎。</br></br>九三 征凶貞厲。革言三就。有孚。</br></br>九四 悔亡有孚。改命吉。</br></br>九五 大人虎變。未占有孚。</br></br>上六 君子豹變。小人革面。征凶。居貞吉。</br></br>",
		"大過 棟撓。利有攸往。亨。</br></br>初六 藉用白茅。无咎。</br></br>九二 枯楊生稊。老夫得其女妻。无不利。</br></br>九三 棟橈。凶。</br></br>九四 棟隆。吉。有它吝。</br></br>九五 枯楊生華。老婦得其士夫。无咎无譽。</br></br>上六 過涉滅頂。凶。无咎。</br></br>",
		"夬 揚于王庭。孚號有厲。告自邑。不利即戎。利有攸往。</br></br>初九 壯于前趾。往不勝為咎。</br></br>九二 惕號。莫夜有戎。勿恤。</br></br>九三 壯于頄。有凶。君子夬夬。獨行遇雨。若濡有慍。无咎。</br></br>九四 臀无膚。其行次且。牽羊悔亡。聞言不信。</br></br>九五 莧陸夬夬。中行无咎。</br></br>上六 无號。終有凶。</br></br>",
		"剝 不利有攸往。</br></br>初六 剝牀以足。蔑貞凶。</br></br>六二 剝牀以辨。蔑貞凶。</br></br>六三 剝之无咎。</br></br>六四 剝牀以膚。凶。</br></br>六五 貫魚。以宮人寵。无不利。</br></br>上九 碩果不食。君子得輿。小人剝廬。</br></br>",
		"頤 貞吉。觀頤。自求口實。</br></br>初九 舍爾靈龜。觀我朶頤。凶。</br></br>六二 顛頤。拂經于丘。頤征凶。</br></br>六三 拂頤。貞凶。十年勿用。无攸利。</br></br>六四 顛頤。吉。虎視眈眈。其欲逐逐。无咎。</br></br>六五 拂經。居貞吉。不可涉大川。</br></br>上九 由頤。厲吉。利涉大川。</br></br>",
		"蒙 亨。匪我求童蒙。童蒙求我。初筮告。再三瀆。瀆則不告。利貞。</br></br>初六 發蒙。利用刑人。用說桎梏。以往吝。</br></br>九二 包蒙吉。納婦吉。子克家。</br></br>六三 勿用取女。見金夫。不有躬。无攸利。</br></br>六四 困蒙。吝。</br></br>六五 童蒙。吉。</br></br>上九 擊蒙。不利為寇。利禦寇。</br></br>",
		"損 有孚。元吉。无咎可貞。利有攸往。曷之用。二簋可用享。</br></br>初九 已事遄往。无咎。酌損之。</br></br>九二 利貞。征凶。弗損益之。</br></br>六三 三人行。則損一人。一人行。則得其友。</br></br>六四 損其疾。使遄有喜。无咎。</br></br>六五 或益之十朋之龜。弗克違。元吉。</br></br>上九 弗損益之。无咎。貞吉。利有攸往。得臣无家。</br></br>",
		"艮 其背。不獲其身。行其庭。不見其人。无咎。</br></br>初六 艮其趾。无咎。利永貞。</br></br>六二 艮其腓。不拯其隨。其心不快。</br></br>九三 艮其限。列其夤。厲熏心。</br></br>六四 艮其身。无咎。</br></br>六五 艮其輔。言有序。悔亡。</br></br>上九 敦艮吉。</br></br>",
		"賁 亨。小利有攸往。</br></br>初九 賁其趾。舍車而徒。</br></br>六二 賁其須。</br></br>九三 賁如濡如。永貞吉。</br></br>六四 賁如皤如。白馬翰如。匪寇婚媾。</br></br>六五 賁于丘園。束帛戔戔。吝。終吉。</br></br>上九 白賁。无咎。</br></br>",
		"蠱 元亨。利涉大川。先甲三日。後甲三日。</br></br>初六 幹父之蠱。有子。考无咎。厲終吉。</br></br>九二 幹母之蠱。不可貞。</br></br>九三 幹父之蠱。小有悔。无大咎。</br></br>六四 裕父之蠱。往見吝。</br></br>六五 幹父之蠱。用譽。</br></br>上九 不事王侯。高尚其事。</br></br>",
		"大畜 利貞。不家食。吉。利涉大川。</br></br>初九 有厲。利已。</br></br>九二 輿說輹。</br></br>九三 良馬逐。利艱貞。曰閑輿衛。利有攸往。</br></br>六四 童牛之牿。元吉。</br></br>六五 豶豕之牙。吉。</br></br>上九 何天之衢。亨。</br></br>",
		"晉 康侯用錫馬蕃庶。晝日三接。</br></br>初六 晉如摧如。貞吉。罔孚。裕无咎。</br></br>六二 晉如愁如。貞吉。受茲介福。于其王母。</br></br>六三 眾允悔亡。</br></br>九四 晉如鼫鼠。貞厲。</br></br>六五 悔亡。失得勿恤。往吉无不利。</br></br>上九 晉其角。維用伐邑。厲吉无咎。貞吝。</br></br>",
		"噬嗑 亨。利用獄。</br></br>初九 履校滅趾。无咎。</br></br>六二 噬膚滅鼻。无咎。</br></br>六三 噬腊肉。遇毒。小吝。无咎。</br></br>九四 噬乾胏。得金矢。利艱貞。吉。</br></br>六五 噬乾肉。得黃金。貞厲。无咎。</br></br>上九 何校滅耳。凶。</br></br>",
		"未濟 亨。小狐汔濟。濡其尾。无攸利。</br></br>初六 濡其尾。吝。</br></br>九二 曳其輪。貞吉。</br></br>六三 未濟征凶。利涉大川。</br></br>九四 貞吉悔亡。震用伐鬼方。三年有賞于大國。</br></br>六五 貞吉无悔。君子之光。有孚吉。</br></br>上九 有孚于飲酒。无咎。濡其首。有孚失是。</br></br>",
		"睽 小事吉。</br></br>初九 悔亡。喪馬勿逐自復。見惡人。无咎。</br></br>九二 遇主于巷。无咎。</br></br>六三 見輿曳。其牛掣。其人天且劓。无初有終。</br></br>九四 睽孤。遇元夫。交孚。厲无咎。</br></br>六五 悔亡。厥宗噬膚。往何咎。</br></br>上九 睽孤。見豕負塗。載鬼一車。先張之弧。後說之弧。匪寇婚媾。往遇雨則吉。</br></br>",
		"旅 小亨。旅貞吉。</br></br>初六 旅瑣瑣。斯其所取災。</br></br>六二 旅即次。懷其資。得童僕貞。</br></br>九三 旅焚其次。喪其童僕。貞厲。</br></br>九四 旅于處。得其資斧。我心不快。</br></br>六五 射雉。一矢亡。終以譽命。</br></br>上九 鳥焚其巢。旅人先笑後號咷。喪牛于易。凶。</br></br>",
		"離 利貞。亨。畜牝牛。吉。</br></br>初九 履錯然。敬之。无咎。</br></br>六二 黃離。元吉。</br></br>九三 日昃之離。不鼓缶而歌。則大耋之嗟。凶。</br></br>九四 突如其來如。焚如。死如。棄如。</br></br>六五 出涕沱若。戚嗟若。吉。</br></br>上九 王用出征。有嘉。折首。獲匪其醜。无咎。</br></br>",
		"鼎 元吉。亨。</br></br>初六 鼎顛趾。利出否。得妾以其子。无咎。</br></br>九二 鼎有實。我仇有疾。不我能即。吉。</br></br>九三 鼎耳革。其行塞。雉膏不食。方雨虧悔。終吉。</br></br>九四 鼎折足。覆公餗。其形渥。凶。</br></br>六五 鼎黃耳金鉉。利貞。</br></br>上九 鼎玉鉉。大吉。无不利。</br></br>",
		"大有 元亨。</br></br>初九 无交害。匪咎。艱則无咎。</br></br>九二 大車以載。有攸往。无咎。</br></br>九三 公用亨于天子。小人弗克 。</br></br>九四 匪其彭。无咎。</br></br>六五 厥孚交如。威如。吉。</br></br>上九 自天祐之。吉无不利。</br></br>",
		"觀 盥而不薦。有孚顒若。</br></br>初六 童觀。小人无咎。君子吝。</br></br>六二 闚觀。利女貞。</br></br>六三 觀我生進退。</br></br>六四 觀國之光。利用賓于王。</br></br>九五 觀我生。君子无咎。</br></br>上九 觀其生。君子无咎。</br></br>",
		"益 利有攸往。利涉大川。</br></br>初九 利用為大作。元吉无咎。</br></br>六二 或益之十朋之龜。弗克違。永貞吉。王用享于帝吉。</br></br>六三 益之用凶事。无咎。有孚中行。告公用圭。</br></br>六四 中行。告公從。利用為依遷國。</br></br>九五 有孚惠心。勿問元吉。有孚惠我德。</br></br>上九 莫益之。或擊之。立心勿恆。凶。</br></br>",
		"渙 亨。王假有廟。利涉大川。利貞。</br></br>初六 用拯馬壯吉。</br></br>九二 渙奔其机。悔亡。</br></br>六三 渙其躬。无悔。</br></br>六四 渙其羣元吉。渙有丘。匪夷所思。</br></br>九五 渙汗其大號。渙。王居无咎。</br></br>上九 渙其血。去逖出。无咎。</br></br>",
		"中孚 豚魚吉。利涉大川。利貞。</br></br>初九 虞吉。有他不燕。</br></br>九二 鳴鶴在陰。其子和之。我有好爵。吾與爾靡之。</br></br>六三 得敵。或鼓或罷。或泣或歌。</br></br>六四 月幾望。馬匹亡。无咎。</br></br>九五 有孚攣如。无咎。</br></br>上九 翰音登于天。貞凶。</br></br>",
		"漸 女歸吉。利貞。</br></br>初六 鴻漸于干。小子厲有言。無咎。</br></br>六二 鴻漸于磐。飲食衎衎。吉。</br></br>九三 鴻漸于陸。夫征不復。婦孕不育。凶。利禦寇。</br></br>六四 鴻漸于木。或得其桷。无咎。</br></br>九五 鴻漸于陵。婦三歲不孕。終莫之勝。吉。</br></br>上九 鴻漸于陸。其羽可用為儀。吉。</br></br>",
		"家人 利女貞。</br></br>初九 閑有家。悔亡。</br></br>六二 无攸遂。在中饋。貞吉。</br></br>九三 家人嗃嗃。悔厲吉。婦子嘻嘻。終吝。</br></br>六四 富家大吉。</br></br>九五 王假有家。勿恤吉。</br></br>上九 有孚威如。終吉。</br></br>",
		"巽 小亨。利有攸往。利見大人。</br></br>初六 進退。利武人之貞。</br></br>九二 巽在牀下。用史巫。紛若。吉。无咎。</br></br>九三 頻巽吝。</br></br>六四 悔亡。田獲三品。</br></br>九五 貞吉悔亡。无不利。无初有終。先庚三日。後庚三日。吉。</br></br>上九 巽在牀下。喪其資斧。貞凶。</br></br>",
		"小畜 亨。密雲不雨。自我西郊。</br></br>初九 復自道。何其咎。吉。</br></br>九二 牽復。吉。</br></br>九三 輿說輻。夫妻反目。</br></br>六四 有孚。血去惕出。无咎。</br></br>九五 有孚攣如。富以其鄰。</br></br>上九 既雨既處。尚德載。婦貞厲。月幾望。君子征凶。</br></br>",
		"否 之匪人。不利君子貞。大往小來。</br></br>初六 拔茅茹。以其彙。貞吉。亨。</br></br>六二 包承。小人吉。大人否。亨。</br></br>六三 包羞。</br></br>九四 有命无咎。疇離祉。</br></br>九五 休否。大人吉。其亡其亡。繫于苞桑。</br></br>上九 傾否。先否後喜。</br></br>",
		"无妄 元亨利貞。其匪正有眚。不利有攸往。</br></br>初九 无妄。往吉。</br></br>六二 不耕穫。不菑畬。則利有攸往。</br></br>六三 无妄之災。或繫之牛。行人之得。邑人之災。</br></br>九四 可貞。无咎。</br></br>九五 无妄之疾。勿藥有喜。</br></br>上九 无妄。行有眚。无攸利。</br></br>",
		"訟 有孚。窒惕。中吉。終凶。利見大人。不利涉大川。</br></br>初六 不永所事。小有言。終吉。</br></br>九二 不克訟。歸而逋其邑。人三百戶。无眚。</br></br>六三 食舊德。貞。厲終吉。或從王事。无成。</br></br>九四 不克訟。復即命。渝安貞。吉。</br></br>九五 訟。元吉。</br></br>上九 或錫之鞶帶。終朝三褫之。</br></br>",
		"履 虎尾。不咥人。亨。</br></br>初九 素履往。无咎。</br></br>九二 履道坦坦。幽人貞吉。</br></br>六三 眇能視。跛能履。履虎尾。咥人凶。武人為于大君。</br></br>九四 履虎尾。愬愬終吉。</br></br>九五 夬履。貞厲。</br></br>上九 視履考祥。其旋元吉。</br></br>",
		"遯 亨。小利貞。</br></br>初六 遯尾厲。勿用有攸往。</br></br>六二 執之用黃牛之革。莫之勝說。</br></br>九三 係遯。有疾厲。畜臣妾吉。</br></br>九四 好遯。君子吉。小人否。</br></br>九五 嘉遯貞吉。</br></br>上九 肥遯无不利。</br></br>",
		"同人 于野。亨。利涉大川。利君子貞。</br></br>初九 同人于門。无咎。</br></br>六二 同人于宗。吝。</br></br>九三 伏戎于莽。升其高陵。三歲不興。</br></br>九四 乘其墉。弗克攻。吉。</br></br>九五 同人先號咷而後笑。大師克相遇。</br></br>上九 同人于郊。无悔。</br></br>",
		"姤 女壯。勿用取女。</br></br>初六 繫于金柅。貞吉。有攸往。見凶。羸豕孚蹢躅。</br></br>九二 包有魚。无咎。不利賓。</br></br>九三 臀无膚。其行次且。厲。无大咎。</br></br>九四 包无魚。起凶。</br></br>九五 以杞包瓜。含章。有隕自天。</br></br>上九 姤其角。吝。无咎。</br></br>",
		"乾 元亨利貞。</br></br>初九 潛龍勿用。</br></br>九二 見龍在田。利見大人。</br></br>九三 君子終日乾乾。夕惕若厲。无咎。</br></br>九四 或躍在淵。无咎。</br></br>九五 飛龍在天。利見大人。</br></br>上九 亢龍有悔。</br></br>用九 見羣龍无首。吉。</br></br>",
	};
}