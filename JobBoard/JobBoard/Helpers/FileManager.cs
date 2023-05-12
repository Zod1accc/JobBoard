﻿namespace JobBoard.Helpers
{
	public static class FileManager
	{
		public static string FileSave(string rootPath,string folderPath,IFormFile file)
		{
			string name = file.FileName;

			name = name.Length > 10 ? name.Substring(name.Length-10, 10) : name;

			name = Guid.NewGuid().ToString() + name;

			string savePath = Path.Combine(rootPath,folderPath, name);

			using (FileStream fileStream = new FileStream(savePath,FileMode.Create))
			{
				file.CopyTo(fileStream);
			}

			return name;
		}

		public static void FileDelete(string rootPath,string folderPath,string fileName)
		{
			string deletePath = Path.Combine(rootPath,folderPath,fileName);

			if(System.IO.File.Exists(deletePath))
			{
				System.IO.File.Delete(deletePath);
			}
		}
	}
}
