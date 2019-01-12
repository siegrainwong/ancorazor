#region

using System;
using SqlSugar;

#endregion

namespace Blog.Repository.Sugar
{
    public class DbContext
    {
        private static DbType _dbType;

        /// <summary>
        /// 连接字符串 
        /// Blog.Core
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 数据连接对象 
        /// Blog.Core 
        /// </summary>
        public SqlSugarClient Db { get; }

        /// <summary>
        /// 数据库上下文实例（自动关闭连接）
        /// Blog.Core 
        /// </summary>
        public static DbContext Context => new DbContext();


        /// <summary>
        /// 功能描述:构造函数
        /// 作　　者:Blog.Core
        /// </summary>
        private DbContext() : this(true)
        {

        }

        /// <summary>
        /// 功能描述:构造函数
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnIsAutoCloseConnection">是否自动关闭连接</param>
        private DbContext(bool blnIsAutoCloseConnection)
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("数据库连接字符串为空");
            Db = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = ConnectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,
                IsShardSameThread = true,
                ConfigureExternalServices = new ConfigureExternalServices(),
                MoreSettings = new ConnMoreSettings
                {
                    //IsWithNoLockQuery = true,
                    IsAutoRemoveDataCache = true
                }
            });
        }

        #region 实例方法
        /// <summary>
        /// 功能描述:获取数据库处理对象
        /// 作　　者:Blog.Core
        /// </summary>
        /// <returns>返回值</returns>
        public SimpleClient<T> GetEntityDB<T>() where T : class, new()
        {
            return new SimpleClient<T>(Db);
        }
        /// <summary>
        /// 功能描述:获取数据库处理对象
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="db">db</param>
        /// <returns>返回值</returns>
        public SimpleClient<T> GetEntityDB<T>(SqlSugarClient db) where T : class, new()
        {
            return new SimpleClient<T>(db);
        }

        #region 根据数据库表生产实体类
        /// <summary>
        /// 功能描述:根据数据库表生产实体类
        /// 作　　者:Blog.Core
        /// </summary>       
        /// <param name="strPath">实体类存放路径</param>
        public void CreateClassFileByDBTalbe(string strPath)
        {
            CreateClassFileByDBTalbe(strPath, "Blog.Core.Entity");
        }
        /// <summary>
        /// 功能描述:根据数据库表生产实体类
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        public void CreateClassFileByDBTalbe(string strPath, string strNameSpace)
        {
            CreateClassFileByDBTalbe(strPath, strNameSpace, null);
        }

        /// <summary>
        /// 功能描述:根据数据库表生产实体类
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        public void CreateClassFileByDBTalbe(
            string strPath,
            string strNameSpace,
            string[] lstTableNames)
        {
            CreateClassFileByDBTalbe(strPath, strNameSpace, lstTableNames, string.Empty);
        }

        /// <summary>
        /// 功能描述:根据数据库表生产实体类
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strPath">实体类存放路径</param>
        /// <param name="strNameSpace">命名空间</param>
        /// <param name="lstTableNames">生产指定的表</param>
        /// <param name="strInterface">实现接口</param>
        /// <param name="blnSerializable"></param>
        public void CreateClassFileByDBTalbe(string strPath, string strNameSpace, string[] lstTableNames, string strInterface, bool blnSerializable = false)
        {
            if (lstTableNames != null && lstTableNames.Length > 0)
            {
                Db.DbFirst.Where(lstTableNames).IsCreateDefaultValue().IsCreateAttribute()
                    .SettingClassTemplate(p => @"
                    {using}

                    namespace {Namespace}
                    {
                        {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
                        public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : " : " + strInterface) + @"
                        {
                            public {ClassName}()
                            {
                    {Constructor}
                            }
                    {PropertyName}
                        }
                    }
                    ")
                    .SettingPropertyTemplate(p => @"
                    {SugarColumn}
                    public {PropertyType} {PropertyName}
                    {
                        get
                        {
                            return _{PropertyName};
                        }
                        set
                        {
                            if(_{PropertyName}!=value)
                            {
                                base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                            }
                            _{PropertyName}=value;
                        }
                    }")
                    .SettingPropertyDescriptionTemplate(p => "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p => "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
            else
            {
                Db.DbFirst.IsCreateAttribute().IsCreateDefaultValue()
                    .SettingClassTemplate(p => p = @"
                    {using}

                    namespace {Namespace}
                    {
                        {ClassDescription}{SugarTable}" + (blnSerializable ? "[Serializable]" : "") + @"
                        public partial class {ClassName}" + (string.IsNullOrEmpty(strInterface) ? "" : " : " + strInterface) + @"
                        {
                            public {ClassName}()
                            {
                    {Constructor}
                            }
                    {PropertyName}
                        }
                    }
                    ")
                    .SettingPropertyTemplate(p => p = @"
                    {SugarColumn}
                    public {PropertyType} {PropertyName}
                    {
                        get
                        {
                            return _{PropertyName};
                        }
                        set
                        {
                            if(_{PropertyName}!=value)
                            {
                                base.SetValueCall(" + "\"{PropertyName}\",_{PropertyName}" + @");
                            }
                            _{PropertyName}=value;
                        }
                    }")
                    .SettingPropertyDescriptionTemplate(p => "          private {PropertyType} _{PropertyName};\r\n" + p)
                    .SettingConstructorTemplate(p => "              this._{PropertyName} ={DefaultValue};")
                    .CreateClassFile(strPath, strNameSpace);
            }
        }
        #endregion

        #region 根据实体类生成数据库表
        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity<T>(bool blnBackupTable, params T[] lstEntitys) where T : class, new()
        {
            Type[] lstTypes = null;
            if (lstEntitys != null)
            {
                lstTypes = new Type[lstEntitys.Length];
                for (var i = 0; i < lstEntitys.Length; i++)
                {
                    lstTypes[i] = typeof(T);
                }
            }
            CreateTableByEntity(blnBackupTable, lstTypes);
        }

        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity(bool blnBackupTable, params Type[] lstEntitys)
        {
            if (blnBackupTable)
            {
                Db.CodeFirst.BackupTable().InitTables(lstEntitys); //change entity backupTable            
            }
            else
            {
                Db.CodeFirst.InitTables(lstEntitys);
            }
        }
        #endregion

        #endregion

        #region 静态方法

        /// <summary>
        /// 功能描述:获得一个DbContext
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnIsAutoCloseConnection">是否自动关闭连接（如果为false，则使用接受时需要手动关闭Db）</param>
        /// <returns>返回值</returns>
        public static DbContext GetDbContext(bool blnIsAutoCloseConnection = true)
        {
            return new DbContext(blnIsAutoCloseConnection);
        }

        /// <summary>
        /// 功能描述:设置初始化参数
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strConnectionString">连接字符串</param>
        /// <param name="enmDbType">数据库类型</param>
        public static void Init(string strConnectionString, DbType enmDbType = DbType.SqlServer)
        {
            ConnectionString = strConnectionString;
            _dbType = enmDbType;
        }

        /// <summary>
        /// 功能描述:创建一个链接配置
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnIsAutoCloseConnection">是否自动关闭连接</param>
        /// <param name="blnIsShardSameThread">是否夸类事务</param>
        /// <returns>ConnectionConfig</returns>
        public static ConnectionConfig GetConnectionConfig(bool blnIsAutoCloseConnection = true, bool blnIsShardSameThread = false)
        {
            var config = new ConnectionConfig
            {
                ConnectionString = ConnectionString,
                DbType = _dbType,
                IsAutoCloseConnection = blnIsAutoCloseConnection,
                ConfigureExternalServices = new ConfigureExternalServices(),
                IsShardSameThread = blnIsShardSameThread
            };
            return config;
        }

        /// <summary>
        /// 功能描述:获取一个自定义的DB
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SqlSugarClient GetCustomDB(ConnectionConfig config)
        {
            return new SqlSugarClient(config);
        }
        /// <summary>
        /// 功能描述:获取一个自定义的数据库处理对象
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="sugarClient">sugarClient</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(SqlSugarClient sugarClient) where T : class, new()
        {
            return new SimpleClient<T>(sugarClient);
        }
        /// <summary>
        /// 功能描述:获取一个自定义的数据库处理对象
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="config">config</param>
        /// <returns>返回值</returns>
        public static SimpleClient<T> GetCustomEntityDB<T>(ConnectionConfig config) where T : class, new()
        {
            var sugarClient = GetCustomDB(config);
            return GetCustomEntityDB<T>(sugarClient);
        }
        #endregion
    }
}
