using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BigFileBrowser
{

    /// <summary>
    /// 文件编码分析
    /// </summary>
    class EncodingHelper
    {
        /// <summary>
        /// 支持的编码类型
        /// </summary>
        public static List<Encoding> Encodings = new List<Encoding>
        {
            //Encoding.GetEncoding("UTF-8"),
            //Encoding.GetEncoding("gb2312"),
        };

        static EncodingHelper()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Encodings.Add(Encoding.UTF8);
            Encodings.Add(Encoding.GetEncoding("gb2312"));

            foreach (var encoding in Encoding.GetEncodings())
            {
                if(!Encodings.Contains(encoding.GetEncoding())) Encodings.Add(encoding.GetEncoding());
            }
        }


        /// <summary>
        /// 寻找最初的字符边界偏移值
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int FindCharacterBoundary(Encoding encoding, byte[] data)
        {
            if (encoding == null || data == null) return 0;
            if (encoding.WebName == "utf-8") return FindFirstValidCharacterBoundaryUtf8(data);
            else if (encoding.WebName == "utf-16") return FindFirstValidCharacterBoundaryUtf16(data);
            else if(encoding.WebName=="gb2312")return FindFirstValidCharacterBoundaryGb2312(data);
            else return 0; 
        }

        public static int FindFirstValidCharacterBoundaryUtf8(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                if ((b & 0x80) == 0) // 单字节字符
                {
                    return i;
                }
                else if ((b & 0xE0) == 0xC0) // 双字节字符的起始字节
                {
                    if (i + 1 < bytes.Length && (bytes[i + 1] & 0xC0) == 0x80)
                    {
                        return i;
                    }
                }
                else if ((b & 0xF0) == 0xE0) // 三字节字符的起始字节
                {
                    if (i + 2 < bytes.Length && (bytes[i + 1] & 0xC0) == 0x80 && (bytes[i + 2] & 0xC0) == 0x80)
                    {
                        return i;
                    }
                }
                else if ((b & 0xF8) == 0xF0) // 四字节字符的起始字节
                {
                    if (i + 3 < bytes.Length && (bytes[i + 1] & 0xC0) == 0x80 && (bytes[i + 2] & 0xC0) == 0x80 && (bytes[i + 3] & 0xC0) == 0x80)
                    {
                        return i;
                    }
                }
            }

            return 0; // 没有找到合法的字符边界
        }

        public static int FindFirstValidCharacterBoundaryUtf16(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i += 2)
            {
                if (i + 1 >= bytes.Length)
                {
                    break; // 没有足够的字节组成一个 UTF-16 字符
                }

                // 检查是否是合法的 UTF-16 字符
                ushort codeUnit = (ushort)((bytes[i] << 8) | bytes[i + 1]);
                if ((codeUnit & 0xFC00) != 0xDC00) // 不是代理对的后半部分
                {
                    return i;
                }
            }

            return 0; // 没有找到合法的字符边界
        }

        public static int FindFirstValidCharacterBoundaryGb2312(byte[] bytes)
        {
            var startIndex = 0;
            string tmp0 = Encoding.GetEncoding("gb2312").GetString(bytes, 0, Math.Min(100, bytes.Length));
            string tmp1 = Encoding.GetEncoding("gb2312").GetString(bytes, 1, Math.Min(100, bytes.Length));

            if (getCommonHanNum(tmp1) > getCommonHanNum(tmp0)) startIndex = 1;

            return startIndex;

        }

        /// <summary>
        /// 统计汉字个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetHanNum(string str)
        {
            int count = 0;
            Regex regex = new Regex(@"^[\u4E00-\u9FA5]{0,}$");
            for (int i = 0; i < str.Length; i++)
            {
                if (regex.IsMatch(str[i].ToString()))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 常见汉字个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int getCommonHanNum(string str)
        {
            int count = 0;

            foreach (char c in str)
            {
                string String11 = "一乙二十丁厂七卜人入八九几儿了力乃刀又三于干亏士工土才寸下大丈与万上小口巾山千乞川亿个勺" +
                    "久凡及夕丸么广亡门义之尸弓己已子卫也女飞刃习叉马乡丰王井开夫天无元专云扎艺木五支厅不太犬区历尤友匹车巨" +
                    "牙屯比互切瓦止少日中冈贝内水见午牛手毛气升长仁什片仆化仇币仍仅斤爪反介父从今凶分乏公仓月氏勿欠风丹匀乌" +
                    "凤勾文六方火为斗忆订计户认心尺引丑巴孔队办以允予劝双书幻玉刊示末未击打巧正扑扒功扔去甘世古节本术可丙左" +
                    "厉右石布龙平灭轧东卡北占业旧帅归且旦目叶甲申叮电号田由史只央兄叼叫另叨叹四生失禾丘付仗代仙们仪白仔他斥" +
                    "瓜乎丛令用甩印乐句匆册犯外处冬鸟务包饥主市立闪兰半汁汇头汉宁穴它讨写让礼训必议讯记永司尼民出辽奶奴加召" +
                    "皮边发孕圣对台矛纠母幼丝式刑动扛寺吉扣考托老执巩圾扩扫地扬场耳共芒亚芝朽朴机权过臣再协西压厌在有百存而" +
                    "页匠夸夺灰达列死成夹轨邪划迈毕至此贞师尘尖劣光当早吐吓虫曲团同吊吃因吸吗屿帆岁回岂刚则肉网年朱先丢舌竹" +
                    "迁乔伟传乒乓休伍伏优伐延件任伤价份华仰仿伙伪自血向似后行舟全会杀合兆企众爷伞创肌朵杂危旬旨负各名多争色" +
                    "壮冲冰庄庆亦刘齐交次衣产决充妄闭问闯羊并关米灯州汗污江池汤忙兴宇守宅字安讲军许论农讽设访寻那迅尽导异孙" +
                    "阵阳收阶阴防奸如妇好她妈戏羽观欢买红纤级约纪驰巡寿弄麦形进戒吞远违运扶抚坛技坏扰拒找批扯址走抄坝贡攻赤" +
                    "折抓扮抢孝均抛投坟抗坑坊抖护壳志扭块声把报却劫芽花芹芬苍芳严芦劳克苏杆杠杜材村杏极李杨求更束豆两丽医辰" +
                    "励否还歼来连步坚旱盯呈时吴助县里呆园旷围呀吨足邮男困吵串员听吩吹呜吧吼别岗帐财针钉告我乱利秃秀私每兵估" +
                    "体何但伸作伯伶佣低你住位伴身皂佛近彻役返余希坐谷妥含邻岔肝肚肠龟免狂犹角删条卵岛迎饭饮系言冻状亩况床库" +
                    "疗应冷这序辛弃冶忘闲间闷判灶灿弟汪沙汽沃泛沟没沈沉怀忧快完宋宏牢究穷灾良证启评补初社识诉诊词译君灵即层" +
                    "尿尾迟局改张忌际陆阿陈阻附妙妖妨努忍劲鸡驱纯纱纳纲驳纵纷纸纹纺驴纽奉玩环武青责现表规抹拢拔拣担坦押抽拐" +
                    "拖拍者顶拆拥抵拘势抱垃拉拦拌幸招坡披拨择抬其取苦若茂苹苗英范直茄茎茅林枝杯柜析板松枪构杰述枕丧或画卧事" +
                    "刺枣雨卖矿码厕奔奇奋态欧垄妻轰顷转斩轮软到非叔肯齿些虎虏肾贤尚旺具果味昆国昌畅明易昂典固忠咐呼鸣咏呢岸" +
                    "岩帖罗帜岭凯败贩购图钓制知垂牧物乖刮秆和季委佳侍供使例版侄侦侧凭侨佩货依的迫质欣征往爬彼径所舍金命斧爸" +
                    "采受乳贪念贫肤肺肢肿胀朋股肥服胁周昏鱼兔狐忽狗备饰饱饲变京享店夜庙府底剂郊废净盲放刻育闸闹郑券卷单炒炊" +
                    "炕炎炉沫浅法泄河沾泪油泊沿泡注泻泳泥沸波泼泽治怖性怕怜怪学宝宗定宜审宙官空帘实试郎诗肩房诚衬衫视话诞询" +
                    "该详建肃录隶居届刷屈弦承孟孤陕降限妹姑姐姓始驾参艰线练组细驶织终驻驼绍经贯奏春帮珍玻毒型挂封持项垮挎城" +
                    "挠政赴赵挡挺括拴拾挑指垫挣挤拼挖按挥挪某甚革荐巷带草茧茶荒茫荡荣故胡南药标枯柄栋相查柏柳柱柿栏树要咸威" +
                    "歪研砖厘厚砌砍面耐耍牵残殃轻鸦皆背战点临览竖省削尝是盼眨哄显哑冒映星昨畏趴胃贵界虹虾蚁思蚂虽品咽骂哗咱" +
                    "响哈咬咳哪炭峡罚贱贴骨钞钟钢钥钩卸缸拜看矩怎牲选适秒香种秋科重复竿段便俩贷顺修保促侮俭俗俘信皇泉鬼侵追" +
                    "俊盾待律很须叙剑逃食盆胆胜胞胖脉勉狭狮独狡狱狠贸怨急饶蚀饺饼弯将奖哀亭亮度迹庭疮疯疫疤姿亲音帝施闻阀阁" +
                    "差养美姜叛送类迷前首逆总炼炸炮烂剃洁洪洒浇浊洞测洗活派洽染济洋洲浑浓津恒恢恰恼恨举觉宣室宫宪突穿窃客冠" +
                    "语扁袄祖神祝误诱说诵垦退既屋昼费陡眉孩除险院娃姥姨姻娇怒架贺盈勇怠柔垒绑绒结绕骄绘给络骆绝绞统耕耗艳泰" +
                    "珠班素蚕顽盏匪捞栽捕振载赶起盐捎捏埋捉捆捐损都哲逝捡换挽热恐壶挨耻耽恭莲莫荷获晋恶真框桂档桐株桥桃格校" +
                    "核样根索哥速逗栗配翅辱唇夏础破原套逐烈殊顾轿较顿毙致柴桌虑监紧党晒眠晓鸭晃晌晕蚊哨哭恩唤啊唉罢峰圆贼贿" +
                    "钱钳钻铁铃铅缺氧特牺造乘敌秤租积秧秩称秘透笔笑笋债借值倚倾倒倘俱倡候俯倍倦健臭射躬息徒徐舰舱般航途拿爹" +
                    "爱颂翁脆脂胸胳脏胶脑狸狼逢留皱饿恋桨浆衰高席准座脊症病疾疼疲效离唐资凉站剖竞部旁旅畜阅羞瓶拳粉料益兼烤" +
                    "烘烦烧烛烟递涛浙涝酒涉消浩海涂浴浮流润浪浸涨烫涌悟悄悔悦害宽家宵宴宾窄容宰案请朗诸读扇袜袖袍被祥课谁调" +
                    "冤谅谈谊剥恳展剧屑弱陵陶陷陪娱娘通能难预桑绢绣验继球理捧堵描域掩捷排掉堆推掀授教掏掠培接控探据掘职基著" +
                    "勒黄萌萝菌菜萄菊萍菠营械梦梢梅检梳梯桶救副票戚爽聋袭盛雪辅辆虚雀堂常匙晨睁眯眼悬野啦晚啄距跃略蛇累唱患" +
                    "唯崖崭崇圈铜铲银甜梨犁移笨笼笛符第敏做袋悠偿偶偷您售停偏假得衔盘船斜盒鸽悉欲彩领脚脖脸脱象够猜猪猎猫猛" +
                    "馅馆凑减毫麻痒痕廊康庸鹿盗章竟商族旋望率着盖粘粗粒断剪兽清添淋淹渠渐混渔淘液淡深婆梁渗情惜惭悼惧惕惊惨" +
                    "惯寇寄宿窑密谋谎祸谜逮敢屠弹随蛋隆隐婚婶颈绩绪续骑绳维绵绸绿琴斑替款堪搭塔越趁趋超提堤博揭喜插揪搜煮援" +
                    "裁搁搂搅握揉斯期欺联散惹葬葛董葡敬葱落朝辜葵棒棋植森椅椒棵棍棉棚棕惠惑逼厨厦硬确雁殖裂雄暂雅辈悲紫辉敞" +
                    "赏掌晴暑最量喷晶喇遇喊景践跌跑遗蛙蛛蜓喝喂喘喉幅帽赌赔黑铸铺链销锁锄锅锈锋锐短智毯鹅剩稍程稀税筐等筑策" +
                    "筛筒答筋筝傲傅牌堡集焦傍储奥街惩御循艇舒番释禽腊脾腔鲁猾猴然馋装蛮就痛童阔善羡普粪尊道曾焰港湖渣湿温渴" +
                    "滑湾渡游滋溉愤慌惰愧愉慨割寒富窜窝窗遍裕裤裙谢谣谦属屡强粥疏隔隙絮嫂登缎缓编骗缘瑞魂肆摄摸填搏塌鼓摆携" +
                    "搬摇搞塘摊蒜勤鹊蓝墓幕蓬蓄蒙蒸献禁楚想槐榆楼概赖酬感碍碑碎碰碗碌雷零雾雹输督龄鉴睛睡睬鄙愚暖盟歇暗照跨" +
                    "跳跪路跟遣蛾蜂嗓置罪罩错锡锣锤锦键锯矮辞稠愁筹签简毁舅鼠催傻像躲微愈遥腰腥腹腾腿触解酱痰廉新韵意粮数煎" +
                    "塑慈煤煌满漠源滤滥滔溪溜滚滨粱滩慎誉塞谨福群殿辟障嫌嫁叠缝缠静碧璃墙撇嘉摧截誓境摘摔聚蔽慕暮蔑模榴榜榨" +
                    "歌遭酷酿酸磁愿需弊裳颗嗽蜻蜡蝇蜘赚锹锻舞稳算箩管僚鼻魄貌膜膊膀鲜疑馒裹敲豪膏遮腐瘦辣竭端旗精歉熄熔漆漂" +
                    "漫滴演漏慢寨赛察蜜谱嫩翠熊凳骡缩慧撕撒趣趟撑播撞撤增聪鞋蕉蔬横槽樱橡飘醋醉震霉瞒题暴瞎影踢踏踩踪蝶蝴嘱" +
                    "墨镇靠稻黎稿稼箱箭篇僵躺僻德艘膝膛熟摩颜毅糊遵潜潮懂额慰劈操燕薯薪薄颠橘整融醒餐嘴蹄器赠默镜赞篮邀衡膨" +
                    "雕磨凝辨辩糖糕燃澡激懒壁避缴戴擦鞠藏霜霞瞧蹈螺穗繁辫赢糟糠燥臂翼骤鞭覆蹦镰翻鹰警攀蹲颤瓣爆疆壤耀躁嚼嚷" +
                    "籍魔灌蠢霸露囊罐匕刁丐歹戈夭仑讥冗邓艾夯凸卢叭叽皿凹囚矢乍尔冯玄邦迂邢芋芍吏夷吁吕吆屹廷迄臼仲伦伊肋旭" +
                    "匈凫妆亥汛讳讶讹讼诀弛阱驮驯纫玖玛韧抠扼汞扳抡坎坞抑拟抒芙芜苇芥芯芭杖杉巫杈甫匣轩卤肖吱吠呕呐吟呛吻吭" +
                    "邑囤吮岖牡佑佃伺囱肛肘甸狈鸠彤灸刨庇吝庐闰兑灼沐沛汰沥沦汹沧沪忱诅诈罕屁坠妓姊妒纬玫卦坷坯拓坪坤拄拧拂" +
                    "拙拇拗茉昔苛苫苟苞茁苔枉枢枚枫杭郁矾奈奄殴歧卓昙哎咕呵咙呻咒咆咖帕账贬贮氛秉岳侠侥侣侈卑刽刹肴觅忿瓮肮" +
                    "肪狞庞疟疙疚卒氓炬沽沮泣泞泌沼怔怯宠宛衩祈诡帚屉弧弥陋陌函姆虱叁绅驹绊绎契贰玷玲珊拭拷拱挟垢垛拯荆茸茬" +
                    "荚茵茴荞荠荤荧荔栈柑栅柠枷勃柬砂泵砚鸥轴韭虐昧盹咧昵昭盅勋哆咪哟幽钙钝钠钦钧钮毡氢秕俏俄俐侯徊衍胚胧胎" +
                    "狰饵峦奕咨飒闺闽籽娄烁炫洼柒涎洛恃恍恬恤宦诫诬祠诲屏屎逊陨姚娜蚤骇耘耙秦匿埂捂捍袁捌挫挚捣捅埃耿聂荸莽" +
                    "莱莉莹莺梆栖桦栓桅桩贾酌砸砰砾殉逞哮唠哺剔蚌蚜畔蚣蚪蚓哩圃鸯唁哼唆峭唧峻赂赃钾铆氨秫笆俺赁倔殷耸舀豺豹" +
                    "颁胯胰脐脓逛卿鸵鸳馁凌凄衷郭斋疹紊瓷羔烙浦涡涣涤涧涕涩悍悯窍诺诽袒谆祟恕娩骏琐麸琉琅措捺捶赦埠捻掐掂掖" +
                    "掷掸掺勘聊娶菱菲萎菩萤乾萧萨菇彬梗梧梭曹酝酗厢硅硕奢盔匾颅彪眶晤曼晦冕啡畦趾啃蛆蚯蛉蛀唬唾啤啥啸崎逻崔" +
                    "崩婴赊铐铛铝铡铣铭矫秸秽笙笤偎傀躯兜衅徘徙舶舷舵敛翎脯逸凰猖祭烹庶庵痊阎阐眷焊焕鸿涯淑淌淮淆渊淫淳淤淀" +
                    "涮涵惦悴惋寂窒谍谐裆袱祷谒谓谚尉堕隅婉颇绰绷综绽缀巢琳琢琼揍堰揩揽揖彭揣搀搓壹搔葫募蒋蒂韩棱椰焚椎棺榔" +
                    "椭粟棘酣酥硝硫颊雳翘凿棠晰鼎喳遏晾畴跋跛蛔蜒蛤鹃喻啼喧嵌赋赎赐锉锌甥掰氮氯黍筏牍粤逾腌腋腕猩猬惫敦痘痢" +
                    "痪竣翔奠遂焙滞湘渤渺溃溅湃愕惶寓窖窘雇谤犀隘媒媚婿缅缆缔缕骚瑟鹉瑰搪聘斟靴靶蓖蒿蒲蓉楔椿楷榄楞楣酪碘硼" +
                    "碉辐辑频睹睦瞄嗜嗦暇畸跷跺蜈蜗蜕蛹嗅嗡嗤署蜀幌锚锥锨锭锰稚颓筷魁衙腻腮腺鹏肄猿颖煞雏馍馏禀痹廓痴靖誊漓" +
                    "溢溯溶滓溺寞窥窟寝褂裸谬媳嫉缚缤剿赘熬赫蔫摹蔓蔗蔼熙蔚兢榛榕酵碟碴碱碳辕辖雌墅嘁踊蝉嘀幔镀舔熏箍箕箫舆" +
                    "僧孵瘩瘟彰粹漱漩漾慷寡寥谭褐褪隧嫡缨撵撩撮撬擒墩撰鞍蕊蕴樊樟橄敷豌醇磕磅碾憋嘶嘲嘹蝠蝎蝌蝗蝙嘿幢镊镐稽" +
                    "篓膘鲤鲫褒瘪瘤瘫凛澎潭潦澳潘澈澜澄憔懊憎翩褥谴鹤憨履嬉豫缭撼擂擅蕾薛薇擎翰噩橱橙瓢蟥霍霎辙冀踱蹂蟆螃螟" +
                    "噪鹦黔穆篡篷篙篱儒膳鲸瘾瘸糙燎濒憾懈窿缰壕藐檬檐檩檀礁磷了瞬瞳瞪曙蹋蟋蟀嚎赡镣魏簇儡徽爵朦臊鳄糜癌懦豁" +
                    "臀藕藤瞻嚣鳍癞瀑襟璧戳攒孽蘑藻鳖蹭蹬簸簿蟹靡癣羹鬓攘蠕巍鳞糯譬霹躏髓蘸镶瓤矗";
                if (String11.Contains(c)) count++;
            }

            return count;
        }


        ///// <summary>
        ///// 返回流的编码格式
        ///// </summary>
        ///// <param name="stream"></param>
        ///// <returns></returns>
        //private static Encoding getEncoding(string streamName)
        //{
        //    Encoding encoding = Encoding.Default;
        //    using (Stream stream = new FileStream(streamName, FileMode.Open))
        //    {
        //        MemoryStream msTemp = new MemoryStream();
        //        int len = 0;
        //        byte[] buff = new byte[512];
        //        while ((len = stream.Read(buff, 0, 512)) > 0)
        //        {
        //            msTemp.Write(buff, 0, len);
        //        }
        //        if (msTemp.Length > 0)
        //        {
        //            msTemp.Seek(0, SeekOrigin.Begin);
        //            byte[] PageBytes = new byte[msTemp.Length];
        //            msTemp.Read(PageBytes, 0, PageBytes.Length);
        //            msTemp.Seek(0, SeekOrigin.Begin);
        //            int DetLen = 0;
        //            UniversalDetector Det = new UniversalDetector(null);
        //            byte[] DetectBuff = new byte[4096];
        //            while ((DetLen = msTemp.Read(DetectBuff, 0, DetectBuff.Length)) > 0 && !Det.IsDone())
        //            {
        //                Det.HandleData(DetectBuff, 0, DetectBuff.Length);
        //            }
        //            Det.DataEnd();
        //            if (Det.GetDetectedCharset() != null)
        //            {
        //                encoding = Encoding.GetEncoding(Det.GetDetectedCharset());
        //            }
        //        }
        //        msTemp.Close();
        //        msTemp.Dispose();
        //        return encoding;
        //    }
        //}



        /// <summary>
        /// 根据字节转化为对应的字符串，并且避免（多字节码的）截断。
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string byteToString(byte[] buffer,int len, Encoding encoding)
        {
            //int len = int.Parse(numericUpDown1.Value.ToString());
            string res = "";
            int begin = 0;
            int end = buffer.Length;
            if (encoding == Encoding.UTF8)
            {
                // 首个多字节符号头
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] >= 192 || buffer[i] < 128)   // 多字节utf-8的首字节或英文字母
                    {
                        begin = i;
                        break;
                    }
                }
                // 最后的多字节符号头（仅检查尾部2字节的余量）
                for (int i = buffer.Length - 1; i > buffer.Length - 3; i--)
                {
                    if (buffer[i] >= 192 || buffer[i] < 128)   // 多字节utf-8的首字节或英文字母
                    {
                        end = i;
                        break;
                    }
                }
            }
            else if (encoding == Encoding.GetEncoding("gb2312"))
            {
                int firstASCII = -1;
                int lastASCII = -1;
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] <= 128)
                    {
                        if (firstASCII < 0) firstASCII = i;
                        lastASCII = i;
                    }
                }
                if (firstASCII < 0)
                {
                    // no ASCII
                    // 选取常见字更多的那一种情况视为正确分割
                    byte[] tmp1 = new byte[buffer.Length - 1];
                    Array.Copy(buffer, 1, tmp1, 0, tmp1.Length);

                    if (getCommonHanNum(encoding.GetString(tmp1)) > getCommonHanNum(encoding.GetString(buffer)))
                    {
                        begin = 1;
                        end = end - 1;
                    }
                }
                else
                {
                    // have ASCII
                    // 两侧如果不是偶数长度，则必有截断。
                    if (firstASCII % 2 != 0) begin = 1;
                    if ((buffer.Length - lastASCII + 1) % 2 != 0) end = end - 1;
                }
                // 防止余量2字节以上，造成重复
                if (end - 1 > len) end -= 2;
            }
            // 防止余量的字节是ASCII码字符时，造成重复
            if (end > len && buffer[end-1] <= 128) end -= 1;

            byte[] buf1 = new byte[end - begin];
            Array.Copy(buffer, begin, buf1, 0, buf1.Length);
            res = encoding.GetString(buf1);
            return res;
        }

        /// <summary>
        /// 对指定bytes测试其所属字符集，返回字符集的编号
        /// </summary>
        /// <param name="testBytes"></param>
        /// <returns></returns>
        public static Encoding AutoChoose(byte[] testBytes)
        {
            //byte[] testBytes = readByte(0, long.Parse(numericUpDown1.Value.ToString()));
            Encoding selectEncoding = Encoding.Default;
            int maxhannum = -1;
            foreach(var encoding in Encodings)
            {
                string str = encoding.GetString(testBytes);
                int thishannum = getCommonHanNum(str);
                if (maxhannum < thishannum)
                {
                    maxhannum = thishannum;
                    selectEncoding = encoding;
                }
            }
            return selectEncoding;
            //this.comboBox1.SelectedIndex = selectindex;
        }

        /// <summary>
        /// 字符是英文字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool isEWord(char c)
        {
            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) return true;
            return false;
        }
    }
}
