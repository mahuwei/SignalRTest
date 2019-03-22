## 拉去 mysql

`docker pull mysql:5.7.25`

## 运行

1. 创建一个 mysql 目录及三个子目录：conf、logs、data
2. 进入 mysql，运行下属命令
   `sudo docker run -p 3306:3306 --name test-mysql -v $PWD/conf:/etc/mysql/conf.d -v $PWD/logs:/logs -v $PWD/data:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=123456 -d mysql:5.7.25 --character-set-server=utf8mb4 --collation-server=utf8mb4_unicode_ci`

## 连接字符串

1. `Server=m-v-m;Database=ef;User=root;Password=123456;Charset=utf8;`
2. "Charset=utf8;"不是必须的，需要 mysql 启动时，添加参数：`--character-set-server=utf8mb4 --collation-server=utf8mb4_unicode_ci` 否则不支持中文

## docker build

`docker build --rm -f "Dockerfile" -t mysqltest:latest .`

## 启动

`docker run -it --rm -p 5000:80 mysqltest:latest`
