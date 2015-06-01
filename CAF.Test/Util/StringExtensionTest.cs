using System;
using System.Collections.Generic;

namespace CAF.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringExtensionTest
    {

        #region PinYin(获取拼音简码)

        /// <summary>
        /// 获取拼音简码
        /// </summary>
        [TestMethod]
        public void TestPinYin()
        {
            string pinyin = null;
            Assert.AreEqual("", pinyin.GetChineseSpell());
            pinyin = "";
            Assert.AreEqual("", pinyin.GetChineseSpell());
            Assert.AreEqual("ZG", ("中国").GetChineseSpell());
            Assert.AreEqual("ZhongGuo", ("中国").ConvertCh());
            Assert.AreEqual("A1BCB2", ("a1宝藏b2").GetChineseSpell());
            Assert.AreEqual("TT", ("饕餮").GetChineseSpell());
        }

        #endregion

        #region Splice(拼接集合元素)

        /// <summary>
        /// 拼接int集合元素，默认用逗号分隔，不带引号
        /// </summary>
        [TestMethod]
        public void TestSplice_Int()
        {
            Assert.AreEqual("1,2,3", (new List<int> { 1, 2, 3 }).Splice());
        }

        /// <summary>
        /// 拼接int集合元素，带单引号
        /// </summary>
        [TestMethod]
        public void TestSplice_Int_SingleQuotes()
        {
            Assert.AreEqual("'1','2','3'", (new List<int> { 1, 2, 3 }).Splice("'"));
        }

        /// <summary>
        /// 拼接int集合元素，空分隔符
        /// </summary>
        [TestMethod]
        public void TestSplice_Int_EmptySeparator()
        {
            Assert.AreEqual("123", (new List<int> { 1, 2, 3 }).Splice("", ""));
        }

        /// <summary>
        /// 拼接int集合元素，带双引号
        /// </summary>
        [TestMethod]
        public void TestSplice_Int_DoubleQuotes()
        {
            Assert.AreEqual("\"1\",\"2\",\"3\"", (new List<int> { 1, 2, 3 }).Splice("\""));
        }

        /// <summary>
        /// 拼接int集合元素，用空格分隔
        /// </summary>
        [TestMethod]
        public void TestSplice_Int_Blank()
        {
            Assert.AreEqual("1 2 3", (new List<int> { 1, 2, 3 }).Splice("", " "));
        }

        /// <summary>
        /// 拼接int集合元素，用分号分隔
        /// </summary>
        [TestMethod]
        public void TestSplice_Int_Semicolon()
        {
            Assert.AreEqual("1;2;3", (new List<int> { 1, 2, 3 }).Splice("", ";"));
        }

        /// <summary>
        /// 拼接字符串集合元素
        /// </summary>
        [TestMethod]
        public void TestSplice_String()
        {
            Assert.AreEqual("1,2,3", (new List<string> { "1", "2", "3" }).Splice());
        }

        /// <summary>
        /// 拼接字符串集合元素，带单引号
        /// </summary>
        [TestMethod]
        public void TestSplice_String_SingleQuotes()
        {
            Assert.AreEqual("'1','2','3'", (new List<string> { "1", "2", "3" }).Splice("'"));
        }

        /// <summary>
        /// 将字符串移除最后一个分隔符并转换为列表
        /// </summary>
        [TestMethod]
        public void TestSplitToList()
        {
            Assert.AreEqual("2", ("1,2,").SplitToList(',')[1]);
            Assert.AreEqual(2, ("1,2,").SplitToList(',').Count);
        }

        /// <summary>
        /// 拼接Guid集合元素
        /// </summary>
        [TestMethod]
        public void TestSplice_Guid()
        {
            Assert.AreEqual("83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A".ToLower(),
                (new List<Guid> {
                    new Guid( "83B0233C-A24F-49FD-8083-1337209EBC9A" ),
                    new Guid( "EAB523C6-2FE7-47BE-89D5-C6D440C3033A" )
                }).Splice(""));
        }

        /// <summary>
        /// 拼接Guid集合元素，带单引号
        /// </summary>
        [TestMethod]
        public void TestSplice_Guid_SingleQuotes()
        {
            Assert.AreEqual("'83B0233C-A24F-49FD-8083-1337209EBC9A','EAB523C6-2FE7-47BE-89D5-C6D440C3033A'".ToLower(),
                (new List<Guid> {
                    new Guid( "83B0233C-A24F-49FD-8083-1337209EBC9A" ),
                    new Guid( "EAB523C6-2FE7-47BE-89D5-C6D440C3033A" )
                }).Splice("'"));
        }

        #endregion

        #region FirstUpper(将值的首字母大写)

        /// <summary>
        /// 将值的首字母大写
        /// </summary>
        [TestMethod]
        public void TestFirstUpper()
        {
            const string text = "aBc";
            string actual = (text).ToCapit();
            Assert.AreEqual("ABc", actual);
        }

        #endregion

        #region ToCamel(将字符串转成驼峰形式)

        /// <summary>
        /// 将字符串转成驼峰形式
        /// </summary>
        [TestMethod]
        public void TestToCamel()
        {
            Assert.AreEqual("aBc", ("ABc").ToCamel());
        }

        #endregion

        /// <summary>
        /// 将字符串转成驼峰形式
        /// </summary>
        [TestMethod]
        public void TestToCapit()
        {
            Assert.AreEqual("ABc", ("_aBc").ToCapit());
        }

        #region ContainsChinese(是否包含中文)

        /// <summary>
        /// 测试是否包含中文
        /// </summary>
        [TestMethod]
        public void TestContainsChinese()
        {
            Assert.IsTrue(("a中1文b").ContainsChinese());
            Assert.IsFalse(("a1b").ContainsChinese());
        }

        #endregion

        #region TestContainsNumber(是否包含数字)

        /// <summary>
        /// 测试是否包含数字
        /// </summary>
        [TestMethod]
        public void TestContainsNumber()
        {
            Assert.IsTrue(("a中1文b").ContainsNumber());
            Assert.IsTrue(("a中2文b").ContainsNumber());
            Assert.IsTrue(("a中3文b").ContainsNumber());
            Assert.IsTrue(("a中4文b").ContainsNumber());
            Assert.IsTrue(("a中5文b").ContainsNumber());
            Assert.IsTrue(("a中6文b").ContainsNumber());
            Assert.IsTrue(("a中7文b").ContainsNumber());
            Assert.IsTrue(("a中8文b").ContainsNumber());
            Assert.IsTrue(("a中9文b").ContainsNumber());
            Assert.IsTrue(("a中0文b").ContainsNumber());
            Assert.IsFalse(("ab").ContainsNumber());
        }

        #endregion

        #region Distinct(去除重复)

        /// <summary>
        /// 去除重复
        /// </summary>
        [TestMethod]
        public void TestDistinct()
        {
            Assert.AreEqual("5", ("55555").Distinct());
            Assert.AreEqual("45", ("45454545").Distinct());
        }

        #endregion

        #region Truncate(截断字符串)

        /// <summary>
        /// 截断字符串
        /// </summary>
        [TestMethod]
        public void TestTruncate()
        {
            string a = null;
            Assert.AreEqual("", a.Truncate(4));
            a = "";
            Assert.AreEqual("", a.Truncate(4));
            Assert.AreEqual("abcd", ("abcdef").Truncate(4));
            Assert.AreEqual("abcd..", ("abcdef").Truncate(4, 2));
            Assert.AreEqual("abcd--", ("abcdef").Truncate(4, 2, "-"));
            Assert.AreEqual("ab", ("ab".Truncate(4)));
        }

        #endregion


        #region GetLastProperty(获取最后一个属性)

        /// <summary>
        /// 获取最后一个属性
        /// </summary>
        [TestMethod]
        public void TestGetLastProperty()
        {
            string a = null;
            Assert.AreEqual("", a.GetLastProperty());
            Assert.AreEqual("", "".GetLastProperty());
            Assert.AreEqual("A", ("A").GetLastProperty());
            Assert.AreEqual("B", ("A.B").GetLastProperty());
            Assert.AreEqual("C", ("A.B.C").GetLastProperty());
        }

        #endregion

        /// <summary>
        /// 实现各进制数间的转换
        /// </summary>
        [TestMethod]
        public void TestConvertBase()
        {
            Assert.AreEqual("00010101", ("21").ConvertBase(10, 2));
            Assert.AreEqual("25", ("21").ConvertBase(10, 8));
            Assert.AreEqual("15", ("21").ConvertBase(10, 16));
        }

        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        [TestMethod]
        public void TestStrLength()
        {
            Assert.AreEqual(5, ("21中r").StrLength());
        }

        /// <summary>
        /// 如果字符串不为空则执行
        /// </summary>
        [TestMethod]
        public void TestStrIsNotNullAction()
        {
            string a = "";
            a.IfIsNullOrEmpty(() => a= "111");
            a.IfIsNotNullOrEmpty(r=> a = "222");
            Assert.AreEqual("222", a);
        }

        /// <summary>
        /// 获取一侧n个字符
        /// </summary>
        [TestMethod]
        public void TestGetLengthOfChars()
        {
            string a = "qwerty";

            Assert.AreEqual("qw", a.Left(2));
            Assert.AreEqual("ty", a.Right(2));
        }

        /// <summary>
        /// 格式化创建字符串
        /// </summary>
        [TestMethod]
        public void TestFormateString()
        {
            string a = "a{0}{1}";
            Assert.AreEqual("abc", a.FormatWith("b", "c"));
        }

        /// <summary>
        /// 格式化创建字符串
        /// </summary>
        [TestMethod]
        public void TestStringEquale()
        {
            string a = "abc";
            Assert.IsTrue(a.IgnoreCaseEqual("Abc"));
        }

        /// <summary>
        /// 返回一个字符串用空格分隔
        /// </summary>
        [TestMethod]
        public void TestWordify()
        {
            string a = "aGoodPeople";
            Assert.AreEqual("a Good People", a.Wordify());
        }

        /// <summary>
        /// 翻转字符串
        /// </summary>
        [TestMethod]
        public void TestReverse()
        {
            string a = "abcde";
            Assert.AreEqual("edcba", a.Reverse());
        }

        /// <summary>
        /// 指定字符串是否在集合中
        /// </summary>
        [TestMethod]
        public void TestIsInArryString()
        {
            string a = "A";
            Assert.IsTrue(a.IsInArryString("A,B,C,D,E", ','));
        }


        /// <summary>
        /// 替换最后一个匹配的字符串
        /// </summary>
        [TestMethod]
        public void TestReplaceLast()
        {
            string a = "ABASDAS";
            Assert.AreEqual("ABASDMS", a.ReplaceLast("A", "M"));
        }


        /// <summary>
        /// 替换第一个匹配的字符串
        /// </summary>
        [TestMethod]
        public void TestReplaceFirst()
        {
            string a = "ABASDAS";
            Assert.AreEqual("MBASDAS", a.ReplaceFirst("A", "M"));
        }

        /// <summary>
        /// 替换第一个匹配的字符串
        /// </summary>
        [TestMethod]
        public void TestCountOccurences()
        {
            string a = "ABASDAS";
            Assert.AreEqual(3, a.CountOccurences("A"));
        }

        /// <summary>
        /// 匹配字符串
        /// </summary>
        [TestMethod]
        public void TestFindSubstringAsString()
        {
            string a = "ABASDAS";
            Assert.AreEqual(2, a.FindSubstringAsString("A.").Count);
            Assert.AreEqual(3, a.FindSubstringAsString("A.", false).Count);//AS子串出现了2次      
            string b = "21_22/21";
            Assert.AreEqual(2, b.FindSubstringAsSInt(@"\d\d").Count);
            Assert.AreEqual(3, b.FindSubstringAsSInt(@"\d{2}", false).Count);//2*子串出现了2次
        }
        /// <summary>
        /// 替换分组字符串
        /// </summary>
        [TestMethod]
        public void TestReplaceReg()
        {
            string b = "21_22/21";
            Assert.AreEqual("2x_2x/2x", b.ReplaceReg(@"(\d)(\d)", "x", 2));
        }
        /// <summary>
        /// 截取包含中文的字符串
        /// </summary>
        [TestMethod]
        public void TestCut()
        {
            string a = "12345678";
            Assert.AreEqual("12345", a.Cut(5));
            string b = "中国1234中国";
            Assert.AreEqual("中国1", b.Cut(5));
        }
        /// <summary>
        /// 转换数字金额主函数（包括小数）
        /// </summary>
        [TestMethod]
        public void TestConvertRMB()
        {
            string a = "12345678.123";
            Assert.AreEqual("壹仟贰佰叁拾肆万伍仟陆佰柒拾捌元壹角贰分", a.ConvertRMB());

        }
    }
}
