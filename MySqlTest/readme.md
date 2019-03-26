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

## 提交镜像(命令可以看本博客命令部分的镜像命令)

`docker commit -m="mysqltest" --author="mahuwei@qq.com" 1109a80c712b mysqltest:v1`
提交命令解释：

1. -m 是对提交的描述，author 是作者(选填)，后面的 1109a80c712b 是修改容器的 id (不是 image id)，后面的是新镜像名字和标签(tag)。
2. 成功之后，会生成新的镜像 id
3. 输入 docker images 查看镜像，会发现新的名为 spencer/django，标签为 v1 的镜像已经存在。

## 综合：

1. docker login --username=your_username registry.cn-beijing.aliyuncs.com
2. docker tag [ImageId] registry.cn-beijing.aliyuncs.com/[命名空间]/[仓库名称]:[镜像版本号]
3. docker push registry.cn-beijing.aliyuncs.com/[命名空间]/[仓库名称]:[镜像版本号]

#

docker build . -t mysqltest:v1
docker run -p 5000:80 mysqltest:v1 -it
