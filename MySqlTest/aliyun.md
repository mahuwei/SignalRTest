## 阿里云容器镜像服务

使用青岛的服务器，张家口连接不一样

## 前置步骤

1.  登录阿里云容器镜像服务
2.  选择青岛服务器
3.  创建“命名空间”
4.  创建“镜像仓库”
5.  选择创建的镜像仓库=> 管理，在操作指南中相关的[操作步骤](https://cr.console.aliyun.com/repository/cn-qingdao/mhw-hubs/mysqltest/details)

## 详细步骤

### 1. 登录阿里云 Docker Registry

`$ sudo docker login --username=mahuwei@qq.com registry.cn-qingdao.aliyuncs.com`

用于登录的用户名为阿里云账号全名，密码为开通服务时设置的密码。

您可以在产品控制台首页修改登录密码

### 2. 从 Registry 中拉取镜像

`$ sudo docker pull registry.cn-qingdao.aliyuncs.com/mhw-hubs/mysqltest:[镜像版本号]`

### 3. 将镜像推送到 Registry

`$ sudo docker login --username=mahuwei@qq.com registry.cn-qingdao.aliyuncs.com`

`$ sudo docker tag [ImageId] registry.cn-qingdao.aliyuncs.com/mhw-hubs/mysqltest:[镜像版本号]`

`$ sudo docker push registry.cn-qingdao.aliyuncs.com/mhw-hubs/mysqltest:[镜像版本号]`

请根据实际镜像信息替换示例中的[ImageId]和[镜像版本号]参数。

### 4. 选择合适的镜像仓库地址

从 ECS 推送镜像时，可以选择使用镜像仓库内网地址。推送速度将得到提升并且将不会损耗您的公网流量。
如果您使用的机器位于经典网络，请使用 `registry-internal.cn-qingdao.aliyuncs.com` 作为 Registry 的域名登录，并作为镜像命名空间前缀。
如果您使用的机器位于 VPC 网络，请使用 `registry-vpc.cn-qingdao.aliyuncs.com` 作为 Registry 的域名登录，并作为镜像命名空间前缀。

## 与 linux 传递文件

1. 使用 pscp,[链接](https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html)
2. window to linux:

   `pscp .\docker-compose.yaml mahuwei@m-v-m:/home/mahuwei/tmp`

3. linux to winows

   `pscp mahuwei@m-v-m:/home/mahuwei/tmp/docker-compose.yaml d:\`

4. 目录复制：

   `pscp -r d:\sql-tmp mahuwei@m-v-m:/home/mahuwei/tmp`
