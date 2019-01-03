using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace WebApplication1.Models
{
    public class Model
    {
        DAO database;
        static int articleNumber;
        public UserInfo PresentUser;
        public Model() {
            database = new MySqlData();
            articleNumber = getArticleNumber();
        }
        //使用者
        public int getArticleNumber()
        {
            return database.getArticleNumber();
        }
        public List<UserInfo> searchAllUsers()
        {
            return database.searchAllUsers();
        }
        public UserInfo searchUser(string name)
        {
            return database.searchUser(name);
        }
        public void addUser(UserInfo newUser)
        {
            database.AddUser(newUser);
            newUser.myBoard.Add(newUser.Name);
            this.addBoard(new Board(newUser.Name,newUser.Name));
        }

        //看板管理
        public Board searchBoard(string name)
        {
            return database.searchBoard(name);
        }
        public void addBoard(Board newBoard)
        {
            database.addBoard(newBoard);
        }
        public void deleteBoard(ref Board board)
        {
            database.deleteBoard(board);            
        }
        public void setBoardManager(ref Board board, string userName)
        {
            board.manager = userName;
            database.setBoardManager(board, userName);
        }

        //文章管理
        public void addArticle(ref Board board, Article article)
        {
            updateArticleNum(1);
            board.article.Add(article);
            database.addArticle(board, article);
        }
        public void addComment(ref Board board, ref Article article, string comment)
        {
            article.comments.Add(comment);
            database.addComment(board, article, comment);
        }
        public void newlikes(ref Board board, ref Article article)
        {
            article.likes++;
            database.newlikes(board, article);
        }


        //好友確認
        public void applyFriend(UserInfo applicant, string friendName)
        {
            database.applyFriend(applicant, friendName);
        }
        public void confirmFriend(ref UserInfo confirmer, string friendName)
        {
            confirmer.friendApply.RemoveAll(item=>item==friendName);
            confirmer.myFriend.Add(friendName);
            database.confirmFriend(confirmer, friendName);
        }
        public int updateArticleNum(int count)
        {
            database.updateArticleNum(count + getArticleNumber());
            return count + getArticleNumber();
        }

       
    }
    public class Article
    {
        public string author;
        public int title;
        public string content;
        public string time;
        public int likes;
        public List<string> comments;
        public Article(int t, string c,string name)
        {
            author = name;
            title=t;content=c;
            time= DateTime.Now.ToString(("yyyy-MM-dd-hh:mm:ss"));
            likes = 0;
            comments = new List<string>();
            //存到database
        }
        public Article() { }
       
    }
    public class Board
    {
        public string Name;
        public string manager;
        public List<Article> article;
        public Board(string name, string managerName)
        {
            Name = name;
            manager = managerName;
            article = new List<Article>();
        }
        public Board() { }
    }
    public class UserInfo
    {
        public string Name;
        public string Password;
        public int coin;
        public bool isManager;
        public List<string> myBoard;
        public List<string> myFriend;
        public List<string> friendApply;
        public UserInfo(string name, string password)
        {
            Name = name;   Password = password;
            coin = 0;
            isManager = false;
            myBoard = new List<string>();
            myFriend = new List<string>();
            friendApply = new List<string>();
        }
        public UserInfo() { }
    }
    public class Manager
    {
        UserInfo user;
        public List<string> BoardApply;
        public List<int> adPrice;
        public Manager()
        {
            BoardApply = new List<string>();
            adPrice = new List<int>();
        }

    }
    public abstract class DAO
    {
        abstract public List<string> getUserFriend(string name);
        abstract public List<string> getUserBoard(string name);
        abstract public List<string> getUserApplicant(string name);
        abstract public List<string> getArticleComment(int title);
        abstract public List<Article> getBoardArticle(string name);
        abstract public List<UserInfo> searchAllUsers();
        abstract public UserInfo searchUser(string name);
        abstract public void AddUser(UserInfo user);
        abstract public Board searchBoard(string name);
        abstract public void applyFriend(UserInfo applicant, string friendName);
        abstract public void confirmFriend(UserInfo confirmer, string friendName);
        abstract public void addBoard(Board newBoard);
        abstract public void deleteBoard(Board board);
        abstract public void addArticle(Board board, Article article);
        abstract public void addComment(Board board, Article article, string comment);
        abstract public void newlikes(Board board, Article article);
        abstract public void setBoardManager(Board board, string userName);
        abstract public int getArticleNumber();
        abstract public void updateArticleNum(int count);
    }
   /* public class H2Data : DAO
    {
        override public int getArticleNumber() { return new int(); }
        override public List<string> getUserFriend(string name) { return new List<string>(); }
        override public List<string> getUserBoard(string name) { return new List<string>(); }
        override public List<string> getUserApplicant(string name) { return new List<string>(); }
        override public List<string> getArticleComment(int title) { return new List<string>(); }
        override public List<Article> getBoardArticle(string name) { return new List<Article>(); }
        override public List<UserInfo> searchAllUsers() { return new List<UserInfo>(); }
        override public UserInfo searchUser(string name)
        {
            return new UserInfo();
        }
        override public void  AddUser(UserInfo user) { }
        override public Board searchBoard(string name)
        {
            return new Board();
        }
        override public void applyFriend(UserInfo applicant, string friendName) { }
        override public void confirmFriend(UserInfo confirmer, string friendName) { }
        override public void addBoard(Board newBoard) { }
        override public void deleteBoard(Board board) { }
        override public void addArticle(Board board, Article article) { }
        override public void addComment(Board board, Article article, string comment) { }
        override public void newlikes(Board board, Article article) { }
        override public void setBoardManager(Board board, string userName) { }
    }*/
    public class MySqlData : DAO
    {
        /*string dbHost = "";//資料庫位址
        string dbUser = "";//資料庫使用者帳號
        string dbPass = "";//資料庫使用者密碼
        string dbName = "";//資料庫名稱*/
        MySqlConnection Connection;
        MySqlCommand command;
        MySqlDataReader reader;
        public MySqlData()
        {
            string connStr = "Database = mydb; Data Source = 192.168.100.11; User Id = root; PassWord ='abc1234567';Charset=utf8;port=3306";
            Connection = new MySqlConnection(connStr);
            command = Connection.CreateCommand();
            Connection.Open();
            command.CommandText = "Create table if not exists userinfo(username longtext,userpassword longtext,  coin int,isManager boolean);";
            command.ExecuteNonQuery();
            command.CommandText = "Create table if not exists Board(name longtext,manager longtext)";
            command.ExecuteNonQuery();
            // Console.ReadLine();
        }
        ~MySqlData()
        {
            Connection.Close();
        }
        override public List<string> getUserFriend(string name)
        {
            List<string> Return = new List<string>();
            command.CommandText = "select * from " + name + "Friend";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string tmp = reader.GetString(0);
                Return.Add(tmp);
            }
            reader.Close();
            return Return;
        }
        override public List<string> getUserBoard(string name)
        {
            List<string> Return = new List<string>();
            command.CommandText = "select * from " + name + "Board";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string tmp = reader.GetString(0);
                Return.Add(tmp);
            }
            reader.Close();
            return Return;
        }
        override public List<string> getUserApplicant(string name)
        {
            List<string> Return = new List<string>();
            command.CommandText = "select * from " + name + "Applicant";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string tmp = reader.GetString(0);
                Return.Add(tmp);
            }
            reader.Close();
            return Return;
        }
        override public List<string> getArticleComment(int title)
        {
            List<string> Return = new List<string>();
            command.CommandText = "select * from " + title.ToString() + "Comment";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string tmp = reader.GetString(0);
                Return.Add(tmp);
            }
            return Return;
        }
        override public List<Article> getBoardArticle(string name)
        {
            List<Article> Return = new List<Article>();
            Article tmp = new Article();
            command.CommandText = "select * from "+name+"Article";
            reader = command.ExecuteReader();
            int title = 0;
            List<string> temp = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                if(!reader.Read()) break;
                if (i == 0) title = reader.GetInt32(0);
                else
                temp.Add(reader.GetString(i));
            }
            while (reader.Read())
            {
                tmp = new Article(title, temp[0], temp[1]);
                tmp.time = temp[2];
                if(!reader.Read()) break;
                tmp.likes = reader.GetInt32(3);
                tmp.comments = getArticleComment(tmp.title);
                Return.Add(tmp);
            }
            reader.Close();
            return Return;
        }
        override public List<UserInfo> searchAllUsers()
        {
            List<UserInfo> returnValue=new List<UserInfo>();
            UserInfo tmp = new UserInfo();
            command.CommandText = "select * from userinfo";
            reader= command.ExecuteReader();
            bool end = false;
            while (true)
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < 2; i++)
                {
                    if (!reader.Read()) { end = true;break; }
                    temp.Add(reader.GetString(i));
                }
                if (end) break;
                tmp = new UserInfo(temp[0], temp[1]);
                reader.Read();
                tmp.coin = reader.GetInt32(2);
                reader.Read();
                tmp.isManager = reader.GetBoolean(3);
                returnValue.Add(tmp);
            }
            reader.Close();
            for (int i = 0; i < returnValue.Count; i++)
            {
                returnValue[i].friendApply = getUserApplicant(returnValue[i].Name);
                returnValue[i].myBoard = getUserBoard(returnValue[i].Name);
                returnValue[i].myFriend = getUserFriend(returnValue[i].Name);
            }
            return returnValue;
        }
        override public UserInfo searchUser(string name)
        {
            UserInfo returnValue=new UserInfo();
            command.CommandText= "select * from userinfo where username = '"+name+"'";
            List<string> tmp = new List<string>();
            reader = command.ExecuteReader(); //execure the reader
            for (int i = 0; i < 2; i++)
            {
                if (reader.Read())
                    tmp.Add(reader.GetString(i));
                else if (tmp.Count > 0) tmp.Add(reader.GetString(i));
                else
                {
                    reader.Close();
                    return returnValue;
                }

            }
            returnValue=new UserInfo(tmp[0], tmp[1]);
            reader.Read();
            returnValue.coin = reader.GetInt32(2);
            reader.Read();
            returnValue.isManager = reader.GetBoolean(3);
            reader.Close();
            returnValue.friendApply = getUserApplicant(name);
            returnValue.myFriend = getUserFriend(name);
            returnValue.myBoard = getUserBoard(name);
            return returnValue;
        }
        override public void AddUser(UserInfo user)
        {
           
            command.CommandText = "Select * from userinfo where username='" + user.Name + "'";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return;
            }
            reader.Close();
            command.CommandText= "Insert into userinfo(username,userpassword,coin,isManager) values('" + user.Name +  "','"+user.Password+"',"+user.coin+","+user.isManager + ")";
            command.ExecuteNonQuery();
            command.CommandText = "Create table if not exists " + user.Name + "Friend(friendname longtext)";
            command.ExecuteNonQuery();
            command.CommandText= "Create table if not exists " + user.Name + "Board(board longtext)";
            command.ExecuteNonQuery();
            command.CommandText = "Insert into '" + user.Name + "Board values ('" + user.Name + "')";
            command.ExecuteNonQuery();
            command.CommandText= "Create table if not exists " + user.Name + "Applicant(applicant longtext)";
            command.ExecuteNonQuery();
        }
        override public Board searchBoard(string name)
        {
            command.CommandText = "Select * from Board where name='" + name + "'";
            reader = command.ExecuteReader();
            reader.Read();
            string Name = reader.GetString(0);
            reader.Read();
            string manager = reader.GetString(1);
            Board Return = new Board(Name, manager);
            reader.Close();
            Return.article = getBoardArticle(Return.Name);
            return Return;
        }
        override public void applyFriend(UserInfo applicant, string friendName)
        {
            command.CommandText = "Insert into " + friendName + "Applicant(appicant) values ('" + applicant.Name + "')";
            command.ExecuteNonQuery();
        }
        override public void confirmFriend(UserInfo confirmer, string friendName)
        {

            command.CommandText = "Insert into " + confirmer.Name + "Friend(friendname) values ('" + friendName + "')";
            command.ExecuteNonQuery();
            command.CommandText="Insert into "+ friendName + "Friend(friendname) values ('" + confirmer.Name + "')";
            command.ExecuteNonQuery();
            command.CommandText = "Delete from " + confirmer.Name + "Applicant where applicant='" + friendName + "'";
            command.ExecuteNonQuery();
        }
        override public void addBoard(Board newBoard)
        {
            command.CommandText = "Select * from board where name='" + newBoard.Name + "'";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return;
            }
            reader.Close();
            command.CommandText = "Create table if not exists " + newBoard.Name + "Article(title int,content longtext,author longtext,time longtext,likes int)";
            command.ExecuteNonQuery();
            command.CommandText = "Insert into Board(name,manager) values ('" + newBoard.Name + "','" + newBoard.manager + "')";
            command.ExecuteNonQuery();
        }
        override public void deleteBoard(Board board)
        {
            command.CommandText = "Delete FROM Board WHERE name='" + board.Name + "'";
            command.ExecuteNonQuery();
            command.CommandText= "Drop table " + board.Name+"Article";
            command.ExecuteNonQuery();
            this.updateArticleNum(-1 * board.article.Count);
            for (int i = 0; i < board.article.Count; i++)
            {
                command.CommandText = "Drop table " + board.article[i].title.ToString() + "Comment";
                command.ExecuteNonQuery();
            }
        }
        override public void addArticle(Board board, Article article)
        {
            command.CommandText = "Select * from " + board.Name + "Article where title=" + article.title;
            int count = command.ExecuteNonQuery();
            if (count != 0) return;
            command.CommandText = "Insert into " + board.Name + "Article(title,content,author,time,likes) values ('" + article.title + "','" + article.content + "','" + article.author + "','" + article.time + "'," + article.likes + ")";
            command.ExecuteNonQuery();
            command.CommandText = "Create table if not exists " + article.title.ToString() + "Comment(comment longtext)";
            command.ExecuteNonQuery();
        }
        override public void addComment(Board board, Article article, string comment)
        {
            command.CommandText = "Insert into " + article.title.ToString() + "Comment(comment) values ('" + comment + "')";
            command.ExecuteNonQuery();
        }
        override public void newlikes(Board board, Article article)
        {
            command.CommandText = "Update " + board.Name + "Article SET likes = " + article.likes + " WHERE title = '" + article.title + "'";
            command.ExecuteNonQuery();
        }
        override public void setBoardManager(Board board, string userName)
        {
            command.CommandText = "Update Board SET manager='" + userName + "'";
            command.ExecuteNonQuery();
            command.CommandText = "Insert into " + userName + "Board(board) values ('" + board.Name + ")";
            command.ExecuteNonQuery();
        }
        public override int getArticleNumber()
        {
            int a;
            command.CommandText = "Select * from ArticleNum";
            reader = command.ExecuteReader();
            reader.Read();
            a = reader.GetInt32(0);
            return a;
        }
        override public void updateArticleNum(int count)
        {
            command.CommandText = "Update ArticleNum SET num="+count;
        }
    }





}