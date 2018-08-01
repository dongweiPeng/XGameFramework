@echo off
rem set csv_path=%1
set origin_excel_dir=%1
set bytes_path=%2
set cs_path=%3
set comdef_path=%4
set tools_dir=%5
set use_origin_excel_dir=%6
set use_csv_export=%7

set csvDir=%tools_dir%\python_protoc\csv
set excel_dir=%tools_dir%\python_protoc\txt
::clear csvDir
del %csvDir%\*.csv /q
if %use_csv_export% GTR 0 (
start /wait csvExporter\Excel2CsvExporter.exe %excel_dir% %csvDir%
)

rem copy /y %csv_path%\*.csv %tools_dir%\python_protoc\txt

rem ---------- build *.proto, *.py and *.bytes ----------------
cd %tools_dir%\python_protoc
xcopy %comdef_path%\commdef.proto protos /s/y/k/x
protoc.exe --proto_path=protos\ --python_out=python_out\ protos\commdef.proto
::start /wait python createProto.py pyexcelBuild
::use py-excel
start /wait python pyexcelBuild.py 0
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
rem del %bytes_path%\*.bytes /q
move /y python_protoc\bytes\*.bytes %bytes_path%\
::clear cs_path
rem del %cs_path%\*.cs /q
move /y csfiles\*.cs %cs_path%\
del protofiles\*.proto
del .\*.proto
pause