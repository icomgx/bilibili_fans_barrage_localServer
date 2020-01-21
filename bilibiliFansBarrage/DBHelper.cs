using System;
using System.Data.SQLite;
using System.Windows.Forms;
using static bilibiliFansBarrage.ErrorUpload;

namespace bilibiliFansBarrage
{
    public class DBHelper
    {
        /// <summary>
        /// sqlite文件路径
        /// </summary>
        private static string dbpath = Application.StartupPath + "\\Data\\bbfb.db3";

        private static string connectionString = "data source = " + dbpath;
        private static SQLiteConnection con = new SQLiteConnection(connectionString);

        /// <summary>
        /// 返回sqlite文件路径
        /// </summary>
        /// <returns>sqlite文件路径</returns>
        public static string RefDBPath()
        {
            return dbpath;
        }

        /// <summary>
        /// 新获取粉丝添加到数据库
        /// </summary>
        /// <param name="mid">b站id</param>
        /// <param name="mtime">关注时间戳</param>
        /// <param name="uname">b站昵称</param>
        /// <param name="face">b站头像</param>
        /// <param name="flag">关注次数</param>
        public static string NewFansToDatabase(string mid, string mtime, string uname, string face, string flag)
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            try{con.Open();}catch (System.Data.SQLite.SQLiteException e){AskUploadError(e.Message.ToString());}
            SQLiteTransaction qLiteTransaction = con.BeginTransaction();
            SQLiteCommand qLiteCommand = new SQLiteCommand();
            qLiteCommand.Connection = con;
            qLiteCommand.Transaction = qLiteTransaction;
            string sqlcmd1 = string.Format("INSERT INTO fans_bc(mid,uname,face,mtime,flag,timeflag,BaCounts,LastBaTime,get_time) VALUES('{0}','{1}','{2}',{3},{4},'{5}','{6}','{7}','{8}')",mid,uname,face,long.Parse(mtime),long.Parse(flag),"0","0",Utils.GetTimeStamp(),DateTime.Now.ToString());
            string sqlcmd2 = string.Format("INSERT INTO fans_temp(mid,uname,face,flag,Bacounts) VALUES('{0}','{1}','{2}','{3}','{4}')",mid,uname,face,flag,"0");
            qLiteCommand.CommandText = sqlcmd1;
            string ref1= qLiteCommand.ExecuteNonQuery().ToString();
            qLiteCommand.CommandText = sqlcmd2;
            string ref2 = qLiteCommand.ExecuteNonQuery().ToString();
            try
            {
                qLiteTransaction.Commit();
                con.Close();
                return ref1 + ref2;
            }
            catch (Exception ex)
            {
                qLiteTransaction.Rollback();
                con.Close();
                return ex.Message;
            }


        }

        public static string FansUpdateToDatabase(string mid, string mtime, string uname, string face)
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            try{con.Open();}catch (System.Data.SQLite.SQLiteException e){AskUploadError(e.Message.ToString());}
            SQLiteTransaction qLiteTransaction = con.BeginTransaction();
            SQLiteCommand qLiteCommand = new SQLiteCommand();
            qLiteCommand.Connection = con;
            qLiteCommand.Transaction = qLiteTransaction;
            string sqlcmd1 = string.Format("UPDATE fans_bc SET flag = (SELECT flag FROM fans_bc WHERE mid = '{0}') + 1 WHERE mid='{0}'",mid);
            string sqlcmd2 = string.Format("UPDATE fans_bc SET uname='{0}' WHERE mid='{1}'",uname,mid);
            string sqlcmd3 = string.Format("UPDATE fans_bc SET face='{0}' WHERE mid='{1}'",face,mid);
            string sqlcmd4 = string.Format("UPDATE fans_bc SET mtime={0} WHERE mid='{1}'",long.Parse(mtime),mid);
            string sqlcmd5 = string.Format("INSERT INTO fans_temp(mid, uname, face, flag, Bacounts) VALUES('{0}', '{1}', '{2}', (SELECT flag FROM fans_bc WHERE mid = '{3}'), '{4}')",mid,uname,face,mid,"0");
            qLiteCommand.CommandText = sqlcmd1;
            string ref1 = qLiteCommand.ExecuteNonQuery().ToString();
            qLiteCommand.CommandText = sqlcmd2;
            string ref2 = qLiteCommand.ExecuteNonQuery().ToString();
            qLiteCommand.CommandText = sqlcmd3;
            string ref3 = qLiteCommand.ExecuteNonQuery().ToString();
            qLiteCommand.CommandText = sqlcmd4;
            string ref4 = qLiteCommand.ExecuteNonQuery().ToString();
            qLiteCommand.CommandText = sqlcmd5;
            string ref5 = qLiteCommand.ExecuteNonQuery().ToString();

            try
            {
                qLiteTransaction.Commit();
                con.Close();
                return ref1 + ref2 + ref3 + ref4 + ref5;
            }

            catch (Exception ex)
            {
                qLiteTransaction.Rollback();
                con.Close();
                return ex.Message;
            }


        }

        /// <summary>
        /// 取出老粉关注次数
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public static string TakeoutBCBus_Flag(string mid)
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            string SqlCmdStr = string.Format("SELECT  flag  FROM Fans_bc WHERE mid='{0}'", mid);
            using (SQLiteCommand cmd = new SQLiteCommand(SqlCmdStr, con))
            {
                try{con.Open();}catch (System.Data.SQLite.SQLiteException e){AskUploadError(e.Message.ToString());}
                var i = cmd.ExecuteScalar();
                con.Close();
                return i.ToString();

            }
        }

        /// <summary>
        /// 取出老粉关注时间戳
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public static string TakeoutBCBus_tamp(string mid)
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            string SqlCmdStr = string.Format("SELECT  mtime  FROM Fans_bc WHERE mid='{0}'", mid);
            using (SQLiteCommand cmd = new SQLiteCommand(SqlCmdStr, con))
            {
                try{con.Open();}catch (System.Data.SQLite.SQLiteException e){AskUploadError(e.Message.ToString());}
                var i = cmd.ExecuteScalar();
                con.Close();
                return i.ToString();

            }
        }





        /// <summary>
        /// 取出最近一个粉丝的昵称
        /// </summary>
        /// <returns></returns>
        public static string TakeoutBC_uname()
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            string SqlCmdStr = "SELECT uname  FROM Fans_temp order by id desc limit 0,1";
            using (SQLiteCommand cmd = new SQLiteCommand(SqlCmdStr, con))
            {
                try{con.Open();}catch (System.Data.SQLite.SQLiteException e){AskUploadError(e.Message.ToString());}
                var i = cmd.ExecuteScalar();
                con.Close();
                return i.ToString();

            }
        }

        /// <summary>
        /// 取出最近一个粉丝的头像
        /// </summary>
        /// <returns></returns>
        public static string TakeoutBC_face()
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            string SqlCmdStr = "SELECT  face  FROM Fans_temp order by id desc limit 0,1";
            using (SQLiteCommand cmd = new SQLiteCommand(SqlCmdStr, con))
            {
                try{con.Open();}catch (System.Data.SQLite.SQLiteException e){AskUploadError(e.Message.ToString());}
                var i = cmd.ExecuteScalar();
                con.Close();
                return i.ToString();

            }
        }



        /// <summary>
        /// 取出最近一个粉丝的Flag
        /// </summary>
        /// <returns></returns>
        public static string TakeoutBC_Flag()
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            string SqlCmdStr = "SELECT  flag  FROM Fans_temp order by id desc limit 0,1";
            using (SQLiteCommand cmd = new SQLiteCommand(SqlCmdStr, con))
            {
                try{con.Open();}catch (System.Data.SQLite.SQLiteException e){AskUploadError(e.Message.ToString());}
                var i = cmd.ExecuteScalar();
                con.Close();
                return i.ToString();

            }
        }

        /// <summary>
        /// 删除Tmep的粉丝数据
        /// </summary>
        /// <returns></returns>
        public static string TakeoutBC_RemoveTemp()
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            string SqlCmdStr = "Delete  from Fans_temp where id =(select  id from Fans_temp order by id desc limit 0,1)";
            using (SQLiteCommand cmd = new SQLiteCommand(SqlCmdStr, con))
            {
                try{con.Open();}catch (System.Data.SQLite.SQLiteException e){AskUploadError(e.Message.ToString());}
                var i = cmd.ExecuteNonQuery();
                con.Close();
                return i.ToString();

            }
        }

        public static string CheckingBC_Mid(string mid)
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            string SqlCmdStr = "SELECT COUNT(*) FROM fans_bc WHERE mid='" + mid + "'";
            using (SQLiteCommand cmd = new SQLiteCommand(SqlCmdStr, con))
            {
                try
                { 
                    con.Open();
                }
                catch (System.Data.SQLite.SQLiteException e)
                {
                    AskUploadError(e.Message.ToString());
                }
                var i = cmd.ExecuteScalar();
                con.Close();
                return i.ToString();

            }
        }


    }
}
