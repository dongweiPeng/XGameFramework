set csv_path=%1
set bytes_path=%2
set cs_path=%3
set comdef_path=%4
set tools_dir=%5

@echo off
rem ---------- build *.proto, *.py and *.bytes ----------------
cd %tools_dir%\python_protoc
xcopy %comdef_path%\commdef.proto protos /s/y/k/x
protoc.exe --proto_path=protos\ --python_out=python_out\ protos\commdef.proto
start /wait python createProto.py
cd ..
cd ..
set SRC_DIR=%tools_dir%\python_protoc\protos
cd %tools_dir%
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