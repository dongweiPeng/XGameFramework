# -*- coding: UTF-8 -*-
import os
import sys
import codecs

infoDir = os.getcwd() + os.sep + 'txt' + os.sep
outDir = os.getcwd() + os.sep + 'protos' + os.sep
pythonOutDir = os.getcwd() + os.sep + 'python_out' + os.sep
cppOutDir = os.getcwd() + os.sep + 'cpp_out' + os.sep
dbpDir = os.getcwd() + os.sep + 'bytes' + os.sep

selcetFileDescriptPath = os.getcwd() + os.sep + 'SelectFiles.txt'

def get_file_list():
##    if os.path.exists(selcetFileDescriptPath):
##        return read_files_from_txt(selcetFileDescriptPath)
##    else:
    ###如果没有"SelectFiles.txt"文件，认为是处理txt目录下所有文件
    return os.listdir(infoDir)

def read_files_from_txt(txtPath):
    select_files = []
    f = codecs.open(txtPath, encoding='utf-8')
    while True:
        line = f.readline()
        if (len(line) == 0) : break
        file_name = line.replace('\r\n', '.csv')
        select_files.append(file_name)
    return select_files

# method for delete files and dirs
def delete_file_folder(src):
    if os.path.isfile(src):
        try:
            if (src.find('__init__.') == -1):
                os.remove(src)
        except:
            pass
    elif os.path.isdir(src):
        for item in os.listdir(src):
            itemsrc=os.path.join(src,item)
            delete_file_folder(itemsrc)
        try:
            os.rmdir(src)
        except:
            pass

# method for clean dir
def cleanDir(sec):
    for item in os.listdir(sec):
        if (item.find('google') != -1):continue
        elif (item.find('commdef') != -1):continue
        elif (item.find('CommonMoudle') != -1):continue
        itemsrc=os.path.join(sec, item)
        delete_file_folder(itemsrc)

# method for making proto file
def makeProtoFile(fileName, splitChar):
    #f = file(infoDir + fileName, 'r')
    f = codecs.open(infoDir + fileName, encoding='GB2312')
    e = file(outDir + fileName[:-4] + '.proto', 'w')
    try:
        line1 = f.readline()
        arr1 = line1.replace('\r', '\n').split('\n')[0].split(splitChar)
     #   print arr1
        line2 = f.readline()
        arr2 = line2.replace('\r', '\n').split('\n')[0].split(splitChar)
     #   print arr2
        line3 = f.readline()
        arr3 = line3.replace('\r', '\n').split('\n')[0].split(splitChar)
     #   print arr3
        #判断是否需要引用外部通用定义 import commdef.proto
        needImportCommon = False
        for typeString in arr3:
            if typeString.find('commdef') != -1:
                needImportCommon = True

        e.write('package dbc;\n')
        if needImportCommon:
            e.write('import \"commdef.proto\";\n')
        e.write('message ' + fileName[:-4] + 'Table \n{\n')
        e.write('\toptional string tname = 1;\n')
        e.write('\trepeated ' + fileName[:-4] + ' tlist = 2;\n')
        pyw = file(pythonOutDir + 'Write_' + fileName[:-4] + '.py', 'w')
        e.write('}\n\n')
        j = 0
        e.write('message ' + fileName[:-4] + '\n{\n')
        msg = '\t'
        for i in arr2:
            if(i.strip() == ''):
                j = j + 1
                continue
            if (i != ''):
                #如果是自定义类型 (暂时只处理enum类型)
                if (arr3[j].find('commdef') != -1):
                    enumDefaultValue = getEnumDefaultValue(arr3[j])
               #     print enumDefaultValue
                    e.write('\toptional ' + arr3[j] + ' ' + i + ' = ' + str(j + 1) + ' [default='+ str(enumDefaultValue) +'];\n')
                    enumStr = parseEnumStr(arr3[j])
                    msg = msg + 'item.' + arr2[j] + ' = ' + 'CommonMoudle.setProtoValue(' + enumStr + ',args[' + str(j) + '], True)\n\t'
                    #msg = msg + 'print(item.' + arr2[j] + ')\n\t'
                else:
                    e.write('\toptional ' + arr3[j] + ' ' + i + ' = ' + str(j + 1) + ';\n')
                    #int类型处理
                    if (arr3[j].find('int') != -1):
                        msg = msg + 'item.' + arr2[j] + ' = ' + 'CommonMoudle.setProtoValue(int, args[' + str(j) + '], False)\n\t'
                    #string类型处理
                    elif (arr3[j].find('string') != -1):
                        msg = msg + 'item.' + arr2[j] + ' = ' + 'CommonMoudle.setProtoValue(str, args[' + str(j) + '], False)\n\t'
                    #bool类型
                    elif (arr3[j].find('bool') != -1):
                        msg = msg + 'item.' + arr2[j] + ' = ' + 'CommonMoudle.setProtoValue(bool, args[' + str(j) + '], False)\n\t'
                    #float类型
                    elif (arr3[j].find('float') != -1):
                        msg = msg + 'item.' + arr2[j] + ' = ' + 'CommonMoudle.setProtoValue(float, args[' + str(j) + '], False)\n\t'
                    #double类型
                    elif (arr3[j].find('double') != -1):
                        msg = msg + 'item.' + arr2[j] + ' = ' + 'CommonMoudle.setProtoValue(float, args[' + str(j) + '], False)\n\t'
                    else:
                        print('Type info is not match! Use the proper google protobuffer type please!')

                j = j + 1
        e.write('}')

        pyw.write('# -*- coding: GB2312 -*-\n')
        pyw.write('import ' + fileName[:-4] + '_pb2\nimport codecs\n\n')
        pyw.write('import CommonMoudle\n\n')
        if needImportCommon:
            pyw.write('import commdef_pb2\n\n')
        pyw.write('def addItem(item, args):\n')
        pyw.write(msg + '\n\n')
        pyw.write('table = ' + fileName[:-4] + '_pb2.' + fileName[:-4] +'Table()\n')
        pyw.write('table.tname = \'' + fileName[:-4] + '.bytes\'\n')
        pyw.write('rf = codecs.open(\'' + infoDir.replace('\\', '\\\\') + fileName + '\', encoding=\'GB2312\')\n')

        splitCh = splitChar
        if '\t' == splitChar:
            splitCh = '\\t'
        mm = '''rf.readline()\nrf.readline()\nrf.readline()\n
while True:
    line = rf.readline()
    if (len(line) == 0):break
    arr = line.replace(\'\\r\', \'\\n\').split(\'\\n\')[0].split(',')
    if (arr[0].strip() == ''):break #Rule that: the first col value should not be none
    addItem(table.tlist.add(), arr)\n'''

        pyw.write(mm)
        pyw.write('\nf = file(\'' + dbpDir.replace('\\', '\\\\') + fileName[:-4]+'.bytes\', \'wb\')\n')
        pyw.write('f.write(table.SerializeToString())\n')
        pyw.write('f.close()\n')
    finally:
        e.close()
        f.close()
        pyw.close()

#只支持类似'commdef.xxxx' 一层'.'
#@return commdef_pb2.SlefEnum
def parseEnumStr(enumStr):
    splitCharPos = enumStr.find('.')
    namespaceStr = enumStr[:splitCharPos]
    classNameStr = enumStr[splitCharPos+1:]
    return namespaceStr +'_pb2.'+classNameStr

#返回enum默认值
def getEnumDefaultValue(enumStr):
    import python_out.commdef_pb2
    splitCharPos = enumStr.find('.')
    classNameStr = enumStr[splitCharPos+1:]
    combineStr = 'python_out.commdef_pb2.' + classNameStr + '.Name(0)'
    defaultVal = eval(combineStr)
    return defaultVal

def makeDBP():
    for item in os.listdir(pythonOutDir):
        if (item.find('Write_') != -1):
            cmd = 'python.exe ' + pythonOutDir + item
            import python_out.commdef_pb2
            os.system(cmd)

# main logic
cleanDir(pythonOutDir) #clean python proto dir
cleanDir(cppOutDir) #clean cpp proto dir
cleanDir(outDir) #clean proto output dir

fileList = get_file_list()
for txtFile in fileList: #make .proto files ,read .csv only
    if txtFile[-4:].lower() == '.csv':
        splitChar = ','
        makeProtoFile(txtFile, splitChar)

cppCmd = 'protoc.exe --cpp_out=' + cppOutDir + ' --proto_path=' + outDir + ' ' + outDir + '*.proto' #define cpp cmd
pyCmd = 'protoc.exe --python_out=' + pythonOutDir + ' --proto_path=' + outDir + ' ' + outDir + '*.proto' #define py cmd

pyresult = os.system(pyCmd) #make proto.py files
cppresult = os.system(cppCmd) #make proto.cc files
if pyresult == 0:
    makeDBP()
else:
    print pyCmd
    print 'python cmd excute failed!'

#cleanDir(pythonOutDir) #clean python proto dir
#cleanDir(cppOutDir) #clean cpp proto dir

print 'press enter key to continue...'
raw_input()