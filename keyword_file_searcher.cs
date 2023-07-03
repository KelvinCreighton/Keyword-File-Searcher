using System;
using System.IO;

namespace Program
{
	public class Program
	{
		
		static void Main()
		{
			Console.Write("Input directory path: ");
			string operatingDirectory = Console.ReadLine();
			if (operatingDirectory == "")
				operatingDirectory = Directory.GetCurrentDirectory();
			Console.Write("Input keyword to search: ");
			string keyword = Console.ReadLine();
			
			Console.WriteLine("----------------------------------------------------------------");
			
			SearchDirectories(operatingDirectory, keyword);
			
			Console.WriteLine("\n\n\n----------------------------------------------------------------");
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
		
		
		public class Tree
		{
			Node rootNode;
			
			public Tree(string root)
			{
				this.rootNode = new Node(root);
			}
			
			public void SearchFiles(string keyword)
			{
				if (Directory.Exists(this.rootNode.dir))
					this.rootNode.SearchFiles(keyword);
				else
					Console.WriteLine("Error: No such directory: " + this.rootNode.dir);
			}
			
			
			public class Node
			{
				public string dir;
				Node[] nodes;
				
				public Node(string dir)
				{
					this.dir = dir;
					this.nodes = null;
				}
				
				public void SearchFiles(string keyword)
				{
					// Get all files in the nodes directory
					string[] currentFiles = Directory.GetFiles(this.dir);
					
					for (int i = 0; i < currentFiles.Length; i++) {
						// Read the contents of each file
						byte[] fileContent = System.IO.File.ReadAllBytes(currentFiles[i]);
						string result = System.Text.Encoding.UTF8.GetString(fileContent);
						
						// Search for keyword
						int[] keywordLines = FindKeywordLines(result, keyword);
						if (keywordLines.Length > 0) {
							// Write the directory and the lines it was found in
							Console.Write("\n\nDirectory: " + currentFiles[i] + "\nLines: ");
							for (int j = 0; j < keywordLines.Length; j++)
								Console.Write(keywordLines[j] + " ");
						}
					}
					
					// Find subdirectories recursively
					string[] subDirectories = Directory.GetDirectories(this.dir);
					this.nodes = new Node[subDirectories.Length];
					for (int i = 0; i < subDirectories.Length; i++) {
						this.nodes[i] = new Node(subDirectories[i]);
						this.nodes[i].SearchFiles(keyword);
					}
				}
			}
		}
		
		
		public static int[] FindKeywordLines(string fileContents, string keyword)
		{
			// Check if the keyword is in the fileContents
			int similarityCounter = 0;
			int lineCounter = 1;
			// Append the lines where the keyword was found to an int arraylist
			System.Collections.Generic.List<int> totalLines = new System.Collections.Generic.List<int>();
			
			for (int i = 0; i < fileContents.Length; i++) {
				if (fileContents[i] == '\n')
					lineCounter += 1;
				
				if (fileContents[i] == keyword[similarityCounter])
					similarityCounter += 1;
				else
					similarityCounter = 0;
				
				if (similarityCounter == keyword.Length) {
					totalLines.Add(lineCounter);
					similarityCounter = 0;
				}
			}
			
			return totalLines.ToArray();
		}
		
		
		public static string SearchDirectories(string dir, string keyword)
		{
			try {
				Tree tree = new Tree(dir);
				tree.SearchFiles(keyword);
			} catch (Exception e) {
				return e.ToString();
			}
			
			return "Successfully Searched Files";
		}
	}
}