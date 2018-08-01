::vresion: 2016-5-9, 用python直接读取excel文件，不做csv的导出过程
::@asvo, 采用的xlrd工具在读excel的时候，会将本是int型的数据读成带小数的数据。如800-> 800.0。在找到合适的读取工具之前，暂时不用这种方法
@echo off
set origin_excel_dir=%1
set bytes_path=%2
set cs_path=%3
set comdef_path=%4
set tools_dir=%5
set use_origin_excel_dir=%6

::将数据拷贝到\txt目录下
set pyExcelDir=%tools_dir%\python_protoc\txt
set excel_dir=%tools_dir%\tempExcelDir
::clear excels
del %pyExcelDir%\*.xlsx /q
del %pyExcelDir%\*.xls /q
if %use_origin_excel_dir% GTR 0 (
copy /y %origin_excel_dir%\*.xlsx %pyExcelDir%\
copy /y %origin_excel_dir%\*.xls %pyExcelDir%\
) else (
copy /y %excel_dir%\*.xlsx %pyExcelDir%\
copy /y %excel_dir%\*.xls %pyExcelDir%\
)

rem ---------- build *.proto, *.py and *.bytes ----------------
cd %tools_dir%\python_protoc
xcopy %comdef_path%\commdef.proto protos /s/y/k/x
protoc.exe --proto_path=protos\ --python_out=python_out\ protos\commdef.proto
start /wait python buildData.py
cd ..
cd ..
set SRC_DIR=%tools_dir%\python_protoc\protos
cd %tools_dir%
::clear dirs
del protofiles\*.proto
del csfiles\*.cs

move /y %SRC_DIR%\*.proto protofiles
copy /y .\protofiles\commdef.proto .\
rem ------- build *.cs --------------
@echo off&setlocal enabledelayedexpansion
for /r %%i in (protofiles\*.proto) do (
set filename=%%~ni
set name=%%~nxi
protogen -i:protofiles\!name! -o:csfiles\!filename!.cs
)

::clear bytes_path
move /y python_protoc\bytes\*.bytes %bytes_path%\
::clear cs_path
move /y csfiles\*.cs %cs_path%\
del protofiles\*.proto
del .\*.proto
pause