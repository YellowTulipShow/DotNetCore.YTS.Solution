using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Collections.Generic;

using YTS.Logic.IO;

namespace YTS.Logic.Test.IO
{
    [TestClass]
    public class TestFilePathExtend
    {
        [TestMethod]
        public void Test_GetRelativePath_AllCases()
        {
            string _test(string cpath, string tpath)
            {
                return FilePathExtend.GetRelativePath(cpath, tpath);
            }

            // 用例 1: 当前路径和目标路径完全相同
            {
                string currentDirectory = "/home/user/docs";
                string targetFilePath = "/home/user/docs";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("./", result, "用例1失败：当前路径和目标路径相同，应返回 './'");
            }

            // 用例 2: 目标路径在当前路径的子目录中
            {
                string currentDirectory = "/home/user/docs";
                string targetFilePath = "/home/user/docs/project/file.txt";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("./project/file.txt", result, "用例2失败：目标路径为子目录，应返回相对路径 './project/file.txt'");
            }

            // 用例 3: 当前路径在目标路径的子目录中
            {
                string currentDirectory = "/home/user/docs/project";
                string targetFilePath = "/home/user/docs";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("../", result, "用例3失败：当前路径在目标路径的子目录，应返回 '../'");
            }

            // 用例 4: 当前路径和目标路径在同一级目录中
            {
                string currentDirectory = "/home/user/docs";
                string targetFilePath = "/home/user/images";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("../images", result, "用例4失败：当前路径和目标路径为兄弟目录，应返回 '../images'");
            }

            // 用例 5: 当前路径和目标路径没有共同的路径
            {
                string currentDirectory = "/home/user/docs";
                string targetFilePath = "/var/www/html";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("/var/www/html", result, "用例5失败：没有共同路径，应返回目标路径本身");
            }

            // 用例 6: 当前路径为根目录
            {
                string currentDirectory = "/";
                string targetFilePath = "/home/user/docs";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("./home/user/docs", result, "用例6失败：当前路径为根目录，应返回目标路径的相对路径");
            }

            // 用例 7: 当前路径和目标路径为空
            {
                string currentDirectory = "";
                string targetFilePath = "";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("./", result, "用例7失败：当前路径和目标路径为空，应返回 './'");
            }

            // 用例 8: 目标路径为空
            {
                string currentDirectory = "/home/user/docs";
                string targetFilePath = "";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("../", result, "用例8失败：目标路径为空，应返回 '../'");
            }

            // 用例 9: 当前路径为空
            {
                string currentDirectory = "";
                string targetFilePath = "/home/user/docs";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("/home/user/docs", result, "用例9失败：当前路径为空，应返回目标路径");
            }

            // 用例 10: 路径中包含多个连续的斜杠
            {
                string currentDirectory = "/home/user/docs/";
                string targetFilePath = "/home/user/docs//project//file.txt";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("./project/file.txt", result, "用例10失败：路径包含多余斜杠，应正常解析并返回 './project/file.txt'");
            }

            // 用例 11: 使用 Windows 风格路径
            {
                string currentDirectory = @"C:\Users\User\Documents";
                string targetFilePath = @"C:\Users\User\Pictures\Image.jpg";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("../Pictures/Image.jpg", result, "用例11失败：Windows 路径解析错误，应返回 '../Pictures/Image.jpg'");
            }

            // 用例 12: 当前路径和目标路径在不同驱动器上
            {
                string currentDirectory = @"C:\Users\User\Documents";
                string targetFilePath = @"D:\Projects\Code";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual(@"D:\Projects\Code", result, "用例12失败：不同驱动器路径，应返回目标路径本身");
            }

            // 用例 13: 路径大小写不同但内容相同
            {
                string currentDirectory = @"/home/user/docs";
                string targetFilePath = @"/HOME/USER/DOCS";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("./", result, "用例13失败：路径大小写不同但内容相同，应返回 './'");
            }

            // 用例 14: 非常长的路径
            {
                string currentDirectory = @"/home/user/docs";
                string targetFilePath = @"/home/user/docs/" + new string('a', 1000) + "/file.txt";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("./" + new string('a', 1000) + "/file.txt", result, "用例14失败：长路径应正确解析");
            }

            // 用例 15: 路径中混合使用不同的分隔符
            {
                string currentDirectory = "/home\\user/docs";
                string targetFilePath = "\\home/user\\images";
                string result = _test(currentDirectory, targetFilePath);
                Assert.AreEqual("../images", result, "用例15失败：路径中混合分隔符应正确解析");
            }


        }
    }
}
