本目录用于存放随安装器一起发布的静态资源。

【职责划分】backend/frontend 完整程序文件不再由本程序复制，改由随后
自动静默调起的 "SVS Lab Tools.msi" 负责安装（见 Core/MsiLauncher.cs、
.aip 工程）。本程序只负责搭建 micromamba 虚拟环境，两条流程互不依赖，
谁先谁后都不会卡住对方——具体顺序是本程序先建环境，全部建完后再调起
MSI 装文件。

MainForm.cs / EnvironmentPlanner.cs 按以下固定相对路径读取（相对于最
终编译出的 SVSLabToolsInstaller.exe 所在目录）：

  Payload\micromamba.exe
      <- 从 https://github.com/mamba-org/micromamba-releases/releases 下载

  Payload\Msi\SVS Lab Tools.msi
      <- 用 Advanced Installer 从 SVS_Lab_Tools.aip 编译出的 MSI，
         负责安装 backend/frontend + 开始菜单快捷方式 + 卸载信息。
         本程序装完虚拟环境后会以
             msiexec /i "...\SVS Lab Tools.msi" /qn
                 APPDIR="<用户选的安装目录>\"
         静默调起它，详见 Core/MsiLauncher.cs。

  Payload\EnvAssets\mfa_env_sitecustomize.py
  Payload\EnvAssets\mfa_utils.py
      <- 从项目 backend 源码里复制出来的两个文件，虚拟环境搭建步骤
         （speechbrain 补丁部署、语言模型下载）直接依赖它们。
         【重要】这里必须是独立的一份拷贝，不能引用 appDir\backend
         下的文件——那份是 MSI 装的，装虚拟环境这一步执行时 MSI 可能
         还没跑完，appDir\backend 可能还不存在。如果以后 backend 源码
         里这两个文件有更新，要记得同步复制一份到这里，两处会不同步
         是这个方案下唯一需要人工留意的地方。
         如果 mfa_utils.py 内部还 import 了 backend 目录下其他自研模块
         （而不只是标准库/第三方包），那些模块也必须一并复制到这个
         目录，否则语言模型下载步骤会在 import 阶段报错——升级前请
         检查一遍 mfa_utils.py 的 import 列表。

.csproj 里 <None Include="Payload\**\*"> 那条规则会在编译时把这个目录
下所有文件自动复制到输出目录（bin\Debug 或 bin\Release），随最终 exe
一起分发。
