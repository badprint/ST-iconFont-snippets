using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iconFont_snippets.StringExtension;

namespace iconFont_snippets
{
    class Program
    {
        static string targetPath = "";
        static int fileCount = 0;

        static void Main(string[] args)
        {
            if(args.Length != 2)
                Console.WriteLine("參數缺少，第一個參數為來為資料夾，第二個參數為目標資料夾");

            var path = args[0];
            targetPath = args[1];

            if (Directory.Exists(path))
                ProcessDirectory(path);
            else
                Console.WriteLine("資料夾不存在！");

            Console.WriteLine("已完成" + fileCount.ToString() + "筆檔案轉換");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.sublime-snippet");
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static void ProcessFile(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);

            using (StreamReader reader = new StreamReader(path))
            {
                string line = string.Empty;

                string text = reader.ReadToEnd().Replace("\n\t", string.Empty).Replace("\n", string.Empty);

                var snippetText = text.GetStrBetweenTags("<content>", "</content>");
                var title = fileName;

                var result = GetSnippetText(title, snippetText);

                var outputPath = Path.Combine(targetPath, fileName + "." + "snippet");

                System.IO.File.WriteAllText(outputPath, result);

                fileCount++;
                Console.WriteLine(outputPath);
            }
        }

        #region 編成文字
        public static string GetSnippetText(string title, string snippetText)
        {
            var format = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                    <CodeSnippets xmlns=""http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet"">;
	                    <CodeSnippet Format=""1.0.0"">
		                    <Header>
			                    <Title>{0}</Title>
			                    <Shortcut>{0}</Shortcut>
			                    <Description>{0}</Description>
			                    <SnippetTypes>
				                    <SnippetType>Expansion</SnippetType>
			                    </SnippetTypes>
		                    </Header>
		                    <Snippet>
			                    <Code Language=""html"">{1}</Code >
		                    </Snippet>
	                    </CodeSnippet>
                    </CodeSnippets>";

            var result = string.Format(format, title, snippetText);

            return result;
        }
        #endregion
    }
}
