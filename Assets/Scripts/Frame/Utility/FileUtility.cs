﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

// 文件工具函数类
public class FileUtility : MathUtility
{
	private static List<string> mTempPatternList = new List<string>();	// 用于避免GC
	private static List<string> mTempFileList = new List<string>();		// 用于避免GC
	private static List<string> mTempFileList1 = new List<string>();	// 用于避免GC
	public static void validPath(ref string path)
	{
		if (isEmpty(path))
		{
			return;
		}
		// 不以/结尾,则加上/
		if (path[path.Length - 1] != '/')
		{
			path += "/";
		}
	}
	public static void releaseFile(byte[] fileBuffer)
	{
		if (fileBuffer == null)
		{
			return;
		}
#if !UNITY_ANDROID || UNITY_EDITOR
		// 非安卓或编辑器下需要使用BytesPool进行回收
		// 如果数组大于等于16KB,则交给GC自动回收,否则就回收到自己的池中
		FrameUtility.UN_ARRAY_THREAD(fileBuffer, fileBuffer.Length >= 1024 * 16);
#else
		// 安卓真机下打开文件时不再进行回收,由GC自动回收
#endif
	}
	// 打开一个二进制文件,fileName为绝对路径,返回值为文件长度
	// 使用完毕后需要使用releaseFile回收文件内存
	public static int openFile(string fileName, out byte[] fileBuffer, bool errorIfNull)
	{
		fileBuffer = null;
		try
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			if (fs == null)
			{
				if (errorIfNull)
				{
					UnityUtility.logError("文件加载失败! : " + fileName);
				}
				return 0;
			}
			int fileSize = (int)fs.Length;
			FrameUtility.ARRAY_THREAD(out fileBuffer, getGreaterPow2(fileSize));
			if (fileBuffer == null)
			{
				fileBuffer = new byte[fileSize];
			}
			fs.Read(fileBuffer, 0, fileSize);
			fs.Close();
			fs.Dispose();
			return fileSize;
#else
			// 安卓平台如果要读取StreamingAssets下的文件,只能使用AssetManager
			if (startWith(fileName, FrameDefine.F_STREAMING_ASSETS_PATH))
			{
				// 改为相对路径
				fileName = fileName.Substring(FrameDefine.F_STREAMING_ASSETS_PATH.Length);
				fileBuffer = AndroidAssetLoader.loadAsset(fileName, errorIfNull);
			}
			// 安卓平台如果要读取persistentDataPath的文件,则可以使用File
			else if (startWith(fileName, FrameDefine.F_PERSISTENT_DATA_PATH))
			{
				fileBuffer = AndroidAssetLoader.loadFile(fileName, errorIfNull);
			}
			else
			{
				UnityUtility.logError("openFile invalid path : " + fileName);
			}
			if (fileBuffer == null && errorIfNull)
			{
				UnityUtility.logError("open file failed! filename : " + fileName);
			}
			return fileBuffer.Length;
#endif
		}
		catch (Exception e)
		{
			if (errorIfNull)
			{
				UnityUtility.logError("文件加载失败! : " + fileName + ", info:" + e.Message + ",stack:" + e.StackTrace);
			}
		}
		return 0;
	}
	// 打开一个文本文件,fileName为绝对路径
	public static string openTxtFile(string fileName, bool errorIfNull)
	{
		int fileSize = openFile(fileName, out byte[] fileContent, errorIfNull);
		if (fileSize == 0)
        {
			return EMPTY;
        }
		string fileBuffer = bytesToString(fileContent, 0, fileSize);
		releaseFile(fileContent);
		return fileBuffer;
	}
	// 打开一个文本文件,fileName为绝对路径,并且自动将文件拆分为多行,移除末尾的换行符(\r或者\n),存储在fileLines中,包含空行,返回值是行数
	public static int openTxtFileLines(string fileName, out string[] fileLines, bool errorIfNull = true, bool keepEmptyLine = true)
	{
		string fileContent = openTxtFile(fileName, errorIfNull);
		if (isEmpty(fileContent))
		{
			fileLines = null;
			return 0;
		}
		splitLine(fileContent, out fileLines, !keepEmptyLine);
		return fileLines != null ? fileLines.Length : 0;
	}
	// 写一个文本文件,fileName为绝对路径,content是写入的字符串
	public static void writeFile(string fileName, byte[] buffer, int size, bool appendData = false)
	{
		// 检测路径是否存在,如果不存在就创建一个
		createDir(getFilePath(fileName));
#if !UNITY_ANDROID || UNITY_EDITOR
		FileStream file = null;
		if (appendData)
		{
			file = new FileStream(fileName, FileMode.Append, FileAccess.Write);
		}
		else
		{
			file = new FileStream(fileName, FileMode.Create, FileAccess.Write);
		}
		file.Write(buffer, 0, size);
		file.Flush();
		file.Close();
		file.Dispose();
#else
		AndroidAssetLoader.writeFile(fileName, buffer, size, appendData);
#endif
	}
	// 写一个文本文件,fileName为绝对路径,content是写入的字符串
	public static void writeTxtFile(string fileName, string content, bool appendData = false)
	{
#if !UNITY_ANDROID || UNITY_EDITOR
		byte[] bytes = stringToBytes(content);
		if (bytes != null)
		{
			writeFile(fileName, bytes, bytes.Length, appendData);
		}
#else
		if(FrameBase.mAndroidAssetLoader != null)
		{
			// 检测路径是否存在,如果不存在就创建一个
			createDir(getFilePath(fileName));
			AndroidAssetLoader.writeTxtFile(fileName, content, appendData);
		}
#endif
	}
	// 重命名文件,参数为绝对路径
	public static bool renameFile(string fileName, string newName)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		UnityUtility.logError("can not rename file on android!");
		return false;
#endif
		if (!isFileExist(fileName) || isFileExist(newName))
		{
			return false;
		}
		Directory.Move(fileName, newName);
		return true;
	}
	// 删除目录,参数为绝对路径
	public static void deleteFolder(string path)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		UnityUtility.logError("can not delete dir on android!");
		return;
#endif
		validPath(ref path);
		if (!isDirExist(path))
		{
			return;
		}
		string[] dirList = Directory.GetDirectories(path);
		// 先删除所有文件夹
		int dirCount = dirList.Length;
		for (int i = 0; i < dirCount; ++i)
		{
			deleteFolder(dirList[i]);
		}
		// 再删除所有文件
		string[] fileList = Directory.GetFiles(path);
		int fileCount = fileList.Length;
		for (int i = 0; i < fileCount; ++i)
		{
			deleteFile(fileList[i]);
		}
		// 再删除文件夹自身
		Directory.Delete(path);
	}
	// 删除path中的所有空目录,参数为绝对路径,deleteSelfIfEmpty表示path本身为空时是否需要删除
	public static bool deleteEmptyFolder(string path, bool deleteSelfIfEmpty = true)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		UnityUtility.logError("can not delete empty dir on android!");
		return false;
#endif
		validPath(ref path);
		// 先删除所有空的文件夹
		string[] dirList = Directory.GetDirectories(path);
		bool isEmpty = true;
		int dirCount = dirList.Length;
		for (int i = 0; i < dirCount; ++i)
		{
			isEmpty = deleteEmptyFolder(dirList[i], true) && isEmpty;
		}
		isEmpty = isEmpty && Directory.GetFiles(path).Length == 0;
		if (isEmpty && deleteSelfIfEmpty)
		{
			Directory.Delete(path);
		}
		return isEmpty;
	}
	// 移动文件,参数为绝对路径
	public static void moveFile(string source, string dest, bool overwrite = true)
	{
		if (!isFileExist(source))
		{
			return;
		}
#if UNITY_ANDROID && !UNITY_EDITOR
		UnityUtility.logError("can not copy file on android!");
		return;
#endif
		if (isFileExist(dest))
		{
			// 先删除目标文件,因为File.Move不支持覆盖文件,目标文件存在时,File.Move会失败
			if (!overwrite)
			{
				return;
			}
			deleteFile(dest);
		}
		else
		{
			// 如果目标文件所在的目录不存在,则先创建目录
			createDir(getFilePath(dest));
		}
		File.Move(source, dest);
	}
	// 拷贝文件,参数为绝对路径
	public static void copyFile(string source, string dest, bool overwrite = true)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		byte[] fileBuffer;
		openFile(source, out fileBuffer, true);
		if(!isFileExist(dest) || overwrite)
		{
			writeFile(dest, fileBuffer, fileBuffer.Length);
		}
#else
		// 如果目标文件所在的目录不存在,则先创建目录
		createDir(getFilePath(dest));
		File.Copy(source, dest, overwrite);
#endif
	}
	// 删除文件,参数为绝对路径
	public static void deleteFile(string path)
	{
		rightToLeft(ref path);
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidAssetLoader.deleteFile(path);
#else
		File.Delete(path);
#endif
	}
	// 获得文件大小,file为绝对路径
	public static int getFileSize(string file)
	{
		rightToLeft(ref file);
		try
		{
#if !UNITY_ANDROID || UNITY_EDITOR
			return (int)new FileInfo(file).Length;
#else
			return AndroidAssetLoader.getFileSize(file);
#endif
		}
		catch(Exception e)
		{
			UnityUtility.logError("getFileSize error! info:" + e.Message + ", file:" + file);
			return 0;
		}
	}
	// 目录是否存在,dir是绝对路径
	public static bool isDirExist(string dir)
	{
		if (isEmpty(dir) || dir == "./" || dir == "../")
		{
			return true;
		}
		validPath(ref dir);
#if !UNITY_ANDROID || UNITY_EDITOR
		return Directory.Exists(dir);
#else
		// 安卓平台如果要读取StreamingAssets下的文件,只能使用AssetManager
		if(startWith(dir, FrameDefine.F_STREAMING_ASSETS_PATH))
		{
			// 改为相对路径
			dir = dir.Substring(FrameDefine.F_STREAMING_ASSETS_PATH.Length);
			return AndroidAssetLoader.isAssetExist(dir);
		}
		// 安卓平台如果要读取persistentDataPath的文件,则可以使用File
		if (startWith(dir, FrameDefine.F_PERSISTENT_DATA_PATH))
		{
			return AndroidAssetLoader.isDirExist(dir);
		}
		UnityUtility.logError("isDirExist invalid path : " + dir);
		return false;
#endif
	}
	// 文件是否存在,fileName为绝对路径
	public static bool isFileExist(string fileName)
	{
#if !UNITY_ANDROID || UNITY_EDITOR
		return File.Exists(fileName);
#else
		// 安卓平台如果要读取StreamingAssets下的文件,只能使用AssetManager
		if (startWith(fileName, FrameDefine.F_STREAMING_ASSETS_PATH))
		{
			// 改为相对路径
			fileName = fileName.Substring(FrameDefine.F_STREAMING_ASSETS_PATH.Length);
			return AndroidAssetLoader.isAssetExist(fileName);
		}
		// 安卓平台如果要读取persistentDataPath的文件,则可以使用File
		if (startWith(fileName, FrameDefine.F_PERSISTENT_DATA_PATH))
		{
			return AndroidAssetLoader.isFileExist(fileName);
		}
		UnityUtility.logError("isFileExist invalid path : " + fileName);
		return false;
#endif
	}
	// 创建文件夹,dir绝对路径
	public static void createDir(string dir)
	{
		if (isDirExist(dir))
		{
			return;
		}
		// 如果有上一级目录,并且上一级目录不存在,则先创建上一级目录
		string parentDir = getFilePath(dir);
		if (parentDir != dir)
		{
			createDir(parentDir);
		}
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidAssetLoader.createDirectory(dir);
#else
		Directory.CreateDirectory(dir);
#endif
	}
	// 查找指定目录下的所有文件,path为GameResources下的相对路径
	public static List<string> findResourcesFilesNonAlloc(string path, string pattern, bool recursive = true, bool keepAbsolutePath = false)
	{
		mTempPatternList.Clear();
		if (!isEmpty(pattern))
		{
			mTempPatternList.Add(pattern);
		}
		mTempFileList.Clear();
		findResourcesFiles(path, mTempFileList, mTempPatternList, recursive, keepAbsolutePath);
		return mTempFileList;
	}
	// 查找指定目录下的所有文件,path为GameResources下的相对路径
	public static List<string> findResourcesFilesNonAlloc(string path, List<string> patterns = null, bool recursive = true, bool keepAbsolutePath = false)
	{
		mTempFileList.Clear();
		findResourcesFiles(path, mTempFileList, patterns, recursive, keepAbsolutePath);
		return mTempFileList;
	}
	// 查找指定目录下的所有文件,path为GameResources下的相对路径
	public static void findResourcesFiles(string path, List<string> fileList, string pattern, bool recursive = true, bool keepAbsolutePath = false)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		UnityUtility.logError("can not find resouces files on android!");
		return;
#endif
		mTempPatternList.Clear();
		if (!isEmpty(pattern))
		{
			mTempPatternList.Add(pattern);
		}
		findResourcesFiles(path, fileList, mTempPatternList, recursive);
	}
	// 查找指定目录下的所有文件,path为GameResources下的相对路径
	public static void findResourcesFiles(string path, List<string> fileList, List<string> patterns = null, bool recursive = true, bool keepAbsolutePath = false)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		UnityUtility.logError("can not find resouces files on android!");
		return;
#endif
		validPath(ref path);
		if (!startWith(path, FrameDefine.F_GAME_RESOURCES_PATH))
		{
			path = FrameDefine.F_GAME_RESOURCES_PATH + path;
		}
		findFiles(path, fileList, patterns, recursive);
		if (!keepAbsolutePath)
		{
			int removeLength = FrameDefine.F_GAME_RESOURCES_PATH.Length;
			int count = fileList.Count;
			for (int i = 0; i < count; ++i)
			{
				fileList[i] = fileList[i].Substring(removeLength);
			}
		}
	}
	// 查找指定目录下的所有文件,path为StreamingAssets下的相对路径
	public static List<string> findStreamingAssetsFilesNonAlloc(string path, string pattern, bool recursive = true, bool keepAbsolutePath = false)
	{
		mTempPatternList.Clear();
		if (!isEmpty(pattern))
		{
			mTempPatternList.Add(pattern);
		}
		mTempFileList.Clear();
		findStreamingAssetsFiles(path, mTempFileList, mTempPatternList, recursive, keepAbsolutePath);
		return mTempFileList;
	}
	// 查找指定目录下的所有文件,path为StreamingAssets下的相对路径
	public static List<string> findStreamingAssetsFilesNonAlloc(string path, List<string> patterns = null, bool recursive = true, bool keepAbsolutePath = false)
	{
		mTempFileList.Clear();
		findStreamingAssetsFiles(path, mTempFileList, patterns, recursive, keepAbsolutePath);
		return mTempFileList;
	}
	// 查找指定目录下的所有文件,path为StreamingAssets下的相对路径
	public static void findStreamingAssetsFiles(string path, List<string> fileList, string pattern, bool recursive = true, bool keepAbsolutePath = false)
	{
		mTempPatternList.Clear();
		if (!isEmpty(pattern))
		{
			mTempPatternList.Add(pattern);
		}
		findStreamingAssetsFiles(path, fileList, mTempPatternList, recursive, keepAbsolutePath);
	}
	// 查找指定目录下的所有文件,path为StreamingAssets下的相对路径,返回的路径列表为绝对路径
	public static void findStreamingAssetsFiles(string path, List<string> fileList, List<string> patterns = null, bool recursive = true, bool keepAbsolutePath = false)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		// 转换为相对路径
		removeStartString(ref path, FrameDefine.F_STREAMING_ASSETS_PATH);
		AndroidAssetLoader.findAssets(path, fileList, patterns, recursive);
		// 查找后的路径本身就是相对路径,如果需要保留绝对路径,则需要将路径加上
		if (keepAbsolutePath)
		{
			int count = fileList.Count;
			for(int i = 0; i < count; ++i)
			{
				fileList[i] = FrameDefine.F_STREAMING_ASSETS_PATH + fileList[i];
			}
		}
#else
		if (!startWith(path, FrameDefine.F_STREAMING_ASSETS_PATH))
		{
			path = FrameDefine.F_STREAMING_ASSETS_PATH + path;
		}
		findFiles(path, fileList, patterns, recursive);
		if (!keepAbsolutePath)
		{
			int removeLength = FrameDefine.F_STREAMING_ASSETS_PATH.Length;
			int count = fileList.Count;
			for (int i = 0; i < count; ++i)
			{
				fileList[i] = fileList[i].Substring(removeLength);
			}
		}
#endif
	}
	// 查找指定目录下的所有文件,path为StreamingAssets下的相对路径,返回的路径列表为绝对路径
	public static void findStreamingAssetsFolders(string path, List<string> folderList, bool recursive = true, bool keepAbsolutePath = false)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		// 转换为相对路径
		removeStartString(ref path, FrameDefine.F_STREAMING_ASSETS_PATH);
		AndroidAssetLoader.findAssetsFolder(path, folderList, recursive);
		// 查找后的路径本身就是相对路径,如果需要保留绝对路径,则需要将路径加上
		if (keepAbsolutePath)
		{
			int count = folderList.Count;
			for(int i = 0; i < count; ++i)
			{
				folderList[i] = FrameDefine.F_STREAMING_ASSETS_PATH + folderList[i];
			}
		}
#else
		// 非安卓平台则查找普通的文件夹
		if (!startWith(path, FrameDefine.F_STREAMING_ASSETS_PATH))
		{
			path = FrameDefine.F_STREAMING_ASSETS_PATH + path;
		}
		findFolders(path, folderList, recursive);
		if (!keepAbsolutePath)
		{
			int removeLength = FrameDefine.F_STREAMING_ASSETS_PATH.Length;
			int count = folderList.Count;
			for (int i = 0; i < count; ++i)
			{
				folderList[i] = folderList[i].Substring(removeLength);
			}
		}
#endif
	}
	// 查找指定目录下的所有文件,path为绝对路径
	public static List<string> findFilesNonAlloc(string path, string pattern, bool recursive = true)
	{
		mTempPatternList.Clear();
		if (!isEmpty(pattern))
		{
			mTempPatternList.Add(pattern);
		}
		mTempFileList1.Clear();
		findFiles(path, mTempFileList1, mTempPatternList, recursive);
		return mTempFileList1;
	}
	// 查找指定目录下的所有文件,path为绝对路径
	public static List<string> findFilesNonAlloc(string path, List<string> patterns = null, bool recursive = true)
	{
		mTempFileList1.Clear();
		findFiles(path, mTempFileList1, patterns, recursive);
		return mTempFileList1;
	}
	// 查找指定目录下的所有文件,path为绝对路径
	public static void findFiles(string path, List<string> fileList, string pattern, bool recursive = true)
	{
		mTempPatternList.Clear();
		if (!isEmpty(pattern))
		{
			mTempPatternList.Add(pattern);
		}
		findFiles(path, fileList, mTempPatternList, recursive);
	}
	// 查找指定目录下的所有文件,path为绝对路径
	public static void findFiles(string path, List<string> fileList, List<string> patterns = null, bool recursive = true)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidAssetLoader.findFiles(path, fileList, patterns, recursive);
#else
		validPath(ref path);
		if (!isDirExist(path))
		{
			return;
		}
		DirectoryInfo folder = new DirectoryInfo(path);
		FileInfo[] fileInfoList = folder.GetFiles();
		int fileCount = fileInfoList.Length;
		int patternCount = patterns != null ? patterns.Count : 0;
		for (int i = 0; i < fileCount; ++i)
		{
			string fileName = fileInfoList[i].Name;
			// 如果需要过滤后缀名,则判断后缀
			if (patternCount > 0)
			{
				for (int j = 0; j < patternCount; ++j)
				{
					if (endWith(fileName, patterns[j], false))
					{
						fileList.Add(path + fileName);
					}
				}
			}
			// 不需要过滤,则直接放入列表
			else
			{
				fileList.Add(path + fileName);
			}
		}
		// 查找所有子目录
		if (recursive)
		{
			string[] dirs = Directory.GetDirectories(path);
			int count = dirs.Length;
			for (int i = 0; i < count; ++i)
			{
				findFiles(dirs[i], fileList, patterns, recursive);
			}
		}
#endif
	}
	// 得到指定目录下的所有第一级子目录,path为绝对路径
	public static bool findFolders(string path, List<string> dirList, bool recursive = true)
	{
		validPath(ref path);
		if (!isDirExist(path))
		{
			return false;
		}
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidAssetLoader.findFolders(path, dirList, recursive);
#else
		// 非安卓平台则查找普通文件夹
		string[] dirs = Directory.GetDirectories(path);
		int count = dirs.Length;
		for (int i = 0; i < count; ++i)
		{
			string dir = dirs[i];
			dirList.Add(dir);
			if (recursive)
			{
				findFolders(dir, dirList, recursive);
			}
		}
#endif
		return true;
	}
	// 计算一个文件的MD5,fileName为绝对路径
	public static string generateFileMD5(string fileName, bool upperOrLower = true)
	{
		int fileSize = openFile(fileName, out byte[] fileContent, true);
		return generateFileMD5(fileContent, fileSize, upperOrLower);
	}
	// 计算一个文件的MD5
	public static string generateFileMD5(byte[] fileContent, int length = -1, bool upperOrLower = true)
	{
		if (length < 0)
		{
			length = fileContent.Length;
		}
		HashAlgorithm algorithm = MD5.Create();
		return bytesToHEXString(algorithm.ComputeHash(fileContent, 0, length), 0, 0, false, upperOrLower);
	}
	// 打开一个文本文件进行处理,然后再写回文本文件
	public static void processFileLine(string fileName, Action<List<string>> process)
	{
		FrameUtility.LIST(out List<string> fileLineList);
		openTxtFileLines(fileName, out string[] fileLines);
		fileLineList.AddRange(fileLines);

		process(fileLineList);

		string fileTxt = stringsToString(fileLineList, "\r\n");
		writeTxtFile(fileName, fileTxt);
		FrameUtility.UN_LIST(fileLineList);
	}
}