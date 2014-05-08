using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Linq;
using Lucene.Net;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Analysis.Standard;
using System.IO;
using System.Web.Hosting;

/// <summary>
/// Summary description for LuceneProvider
/// </summary>
public class LuceneProvider
{
	private static LuceneDataProvider _instance;
	private static ReadOnlyLuceneDataProvider _readOnlyInstance;
	private static DirectoryInfo _appDataFolder;

	static LuceneProvider()
	{
		_appDataFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/BlogIndex"));
		var directory = new SimpleFSDirectory(_appDataFolder);

		IndexWriter writer;
		FSDirectory indexFSDir = FSDirectory.Open(_appDataFolder, new Lucene.Net.Store.SimpleFSLockFactory(_appDataFolder));

		try
		{
			writer = new IndexWriter(directory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED);
		}

		catch (LockObtainFailedException ex)
		{
			IndexWriter.Unlock(indexFSDir);
			writer = new IndexWriter(directory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED);
		}

		_instance = new LuceneDataProvider(directory, writer.Analyzer, Lucene.Net.Util.Version.LUCENE_30, writer);
		_readOnlyInstance = new ReadOnlyLuceneDataProvider(directory, Lucene.Net.Util.Version.LUCENE_30);
	}
	public static LuceneDataProvider Instance
	{
		get
		{
			return _instance;
		}
	}

	public static ReadOnlyLuceneDataProvider ReadOnlyInstance
	{
		get
		{
			return _readOnlyInstance;
		}
	}
	
}