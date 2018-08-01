#-*- coding:utf-8 -*-
import xlrd
import xdrlib, sys
import codecs
import os

#excel文件路径
excel_dir = os.getcwd() + os.sep + 'txt' + os.sep
#输出proto文件路径
proto_dir = os.getcwd() + os.sep + 'protos' + os.sep
#输出write_xxx.py文件路径
write_py_dir = os.getcwd() + os.sep + 'python_out' + os.sep
#生成.bytes文件路径
bytes_dir = os.getcwd() + os.sep + 'bytes' + os.sep

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

def open_excel(filename):
    try:
        data = xlrd.open_workbook(filename)
        return data
    except Exception,e:
        print(e)

def make_protofile(proplist, typelist, sheet_name, need_common_module):
    try:
        e = file(proto_dir + sheet_name + '.proto', 'w')
        e.write('package dbc;\n')
        if need_common_module:
            e.write('import \"commdef.proto\";\n')
        e.write('message ' + sheet_name + 'Table \n{\n')
        e.write('\toptional string tname = 1;\n')
        e.write('\trepeated ' + sheet_name + ' tlist = 2;\n')
        e.write('}\n\n')
        e.write('message ' + sheet_name + '\n{\n')
        for i in range(len(proplist)):
            type_str = typelist[i]
            if type_str.find('commdef') != -1:
                enumDefaultValue = getEnumDefaultValue(type_str)
                e.write('\toptional ' + type_str + ' ' + proplist[i] + ' = ' + str(i + 1) + ' [default='+ str(enumDefaultValue) +'];\n')
            else:
                e.write('\toptional ' + type_str + ' ' + proplist[i] + ' = ' + str(i + 1) + ';\n')
        e.write('}\n\n')
    finally:
        e.close()

def make_writefile(proplist, typelist, sheet_name, excel_file_name, need_common_module):
    try:
        py_filename = 'Write_' + sheet_name + '.py'
        pyw = file(write_py_dir + py_filename, 'w')
        pyw.write('# -*- coding: utf-8 -*-\n')
        pyw.write('import ' + sheet_name + '_pb2\nimport codecs\n\n')
        pyw.write('import CommonMoudle\n\n')
        if need_common_module:
            pyw.write('import commdef_pb2\n\n')
        pyw.write('def addItem(item, args):\n')
        msg = '\t'
        for i in range(len(proplist)):
            type_str = typelist[i]
            if type_str.find('commdef') != -1:
                enumStr = parseEnumStr(type_str)
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValue(' + enumStr + ',args[' + str(i) + '], True)\n\t'
            #int类型处理
            elif type_str.find('int') != -1:
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValue(int, args[' + str(i) + '], False)\n\t'
            #string类型处理
            elif type_str.find('string') != -1:
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValue(str, args[' + str(i) + '], False)\n\t'
            #bool类型处理
            elif type_str.find('bool') != -1:
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValue(bool, args[' + str(i) + '], False)\n\t'
            #float类型处理
            elif type_str.find('float') != -1:
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValue(float, args[' + str(i) + '], False)\n\t'
            #double类型处理
            elif type_str.find('double') != -1:
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValue(flaot, args[' + str(i) + '], False)\n\t'
            else:
                print('asvo-error: Type info is not match! Use the proper google protobuffer type please!')

        pyw.write(msg + '\n\n')
        pyw.write('table = ' + sheet_name + '_pb2.' + sheet_name +'Table()\n')
        pyw.write('table.tname = \'' + sheet_name + '.bytes\'\n')
        pyw.write('rf = CommonMoudle.open_excel(\'' + excel_dir.replace('\\', '\\\\') + excel_file_name + '\')\n')
        pyw.write('sheet = rf.sheet_by_name(\'' + sheet_name + '\')\n')
        mm = '''rows = sheet.nrows\nstart_row = 3\n\n
for i in range(start_row, rows):
    line = sheet.row_values(i)
    if (len(line) == 0):break
    if (line[0] == ''):break
    addItem(table.tlist.add(), line)\n'''
        pyw.write(mm)
        pyw.write('\nf = file(\'' + bytes_dir.replace('\\', '\\\\') + sheet_name +'.bytes\', \'wb\')\n')
        pyw.write('f.write(table.SerializeToString())\n')
        pyw.write('f.close()\n')
    finally:
        pyw.close()

#生成proto文件，write_xx.py文件
def build_excel_file(xls_filename):
    #处理单个excel文件
    data = open_excel(excel_dir + xls_filename)
    sheet_names = data.sheet_names()

    for name in sheet_names:
        #处理每个sheet
        sheet = data.sheet_by_name(name)
        #不足3行的数据不读取
        if sheet.nrows <= 3:    #?????? <=? or <?
            continue
        prop_list = sheet.row_values(1)
        type_list = sheet.row_values(2)
        needImportCommon = False
        for typeString in type_list:
            if typeString.find('commdef') != -1:
                needImportCommon = True
        make_protofile(prop_list, type_list, name, needImportCommon)
        make_writefile(prop_list, type_list, name, xls_filename, needImportCommon)

#执行write_xx.py
def makeDBP():
    for item in os.listdir(write_py_dir):
        if (item.find('Write_') != -1):
            cmd = 'python.exe ' + write_py_dir + item
            import python_out.commdef_pb2
            os.system(cmd)

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

def makeDBP():
    for item in os.listdir(write_py_dir):
        if (item.find('Write_') != -1):
            cmd = 'python.exe ' + write_py_dir + item
            import python_out.commdef_pb2
            os.system(cmd)

cleanDir(write_py_dir) #clean python proto dir
cleanDir(proto_dir) #clean proto output dir

excel_file_list = os.listdir(excel_dir)
for excelfile in excel_file_list:
    print excelfile
    if excelfile[-5:] == '.xlsx':
        build_excel_file(excelfile)
    elif excelfile[-4:] == '.xls':
        build_excel_file(excelfile)

#excute(protogen.exe)
#exceute(write.py)
pyCmd = 'protoc.exe --python_out=' + write_py_dir + ' --proto_path=' + proto_dir + ' ' + proto_dir + '*.proto' #define py cmd

pyresult = os.system(pyCmd) #make proto.py files
if pyresult == 0:
    makeDBP()
else:
    print pyCmd
    print 'python cmd excute failed!'

#cleanDir(pythonOutDir) #clean python proto dir
#cleanDir(cppOutDir) #clean cpp proto dir

print 'press enter key to continue...'
raw_input()
