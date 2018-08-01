@echo off
SET _PROJECT_PATH=E:\wdJ_333\client\WDJ1.0
SET _PROTO_PATH=.\protofiles
SET _NET_FRAMEWORK_PATH=C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe

rem cd .\protoDllBuild

md PreserveCsFiles
md ProtoCSFile
md SerializerGenerator
del /q .\ProtoCSFile\*.*
del /q .\SerializerGenerator\*.*

copy %_PROJECT_PATH%\Assets\Scripts\GenerateCodes\*.cs .\ProtoCSFile\
copy .\PreserveCsFiles\*.cs .\ProtoCSFile\
 
"%_NET_FRAMEWORK_PATH%" /target:library /out:.\ProtoCSFile\WdjDTO.dll /reference:protobuf-net.dll /optimize /warn:4 /define:TRACE /debug:pdbonly .\ProtoCSFile\*.cs
 pause
 
start /wait python generate_serialize_main.py
pause

move .\Program.cs .\SerializerGenerator\

copy .\ProtoCSFile\WdjDTO.dll .\SerializerGenerator\
copy .\protobuf-net.dll .\SerializerGenerator\

"%_NET_FRAMEWORK_PATH%" /target:exe /out:.\SerializerGenerator\SerializerGenerator.exe /reference:protobuf-net.dll /reference:./ProtoCSFile/WdjDTO.dll /optimize /warn:4 /define:TRACE /debug:pdbonly .\SerializerGenerator\*.cs

pause

cd .\SerializerGenerator

start "" ".\SerializerGenerator.exe"

pause

copy /y .\WdjDTO.dll .\protoDllBuild\

copy /y .\WdjDTOSerializer.dll .\protoDllBuild\

pause