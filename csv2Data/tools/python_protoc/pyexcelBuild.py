#-*- coding:utf-8 -*-
import xlrd
import xdrlib, sys
import codecs
import os

##sysencoding = sys.getdefaultencoding()
##if sysencoding != 'utf-8':
##    reload(sys)
##    sys.setdefaultencoding('utf8')

#全改成相对路径
#excel文件路径
#excel_dir = os.getcwd() + os.sep + 'txt' + os.sep
#excel文件相对当前目录python_protoc的路径
excel_dir = 'txt' + os.sep
#csv_dir = os.getcwd() + os.sep + 'csv' + os.sep
csv_dir = 'csv' + os.sep
#输出proto文件路径
#proto_dir = os.getcwd() + os.sep + 'protos' + os.sep
proto_dir = 'protos' + os.sep
#输出write_xxx.py文件路径
#write_py_dir = os.getcwd() + os.sep + 'python_out' + os.sep
write_py_dir = 'python_out' + os.sep
#生成.bytes文件路径
#bytes_dir = os.getcwd() + os.sep + 'bytes' + os.sep
bytes_dir = 'bytes' + os.sep

use_csv_export = sys.argv[1]

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
        if isinstance(filename, unicode):
            filename = filename.encode('utf-8')
        filename = filename.split('\\')[-1:][0]
        filename = excel_dir + filename
        data = xlrd.open_workbook(filename)
        return data
    except Exception,e:
        print(e)
        os.system('pause')

def make_protofile(proplist, typelist, sheet_name, need_common_module):
    debug_type_str = 'default'
    debug_prop_str = 'default'
    try:
        if isinstance(sheet_name, unicode):
            sheet_name = sheet_name.encode('utf-8')
        proto_path = proto_dir + sheet_name
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
            if type_str.strip() == '':
                continue
            debug_type_str = type_str
            debug_prop_str = proplist[i]
            target_type = calculate_prop_type(type_str)
            if type_str.find('commdef') != -1:
                enumDefaultValue = getEnumDefaultValue(type_str)
                e.write('\toptional ' + type_str + ' ' + proplist[i] + ' = ' + str(i + 1) + ' [default='+ str(enumDefaultValue) +'];\n')
            else:
                e.write('\toptional ' + target_type + ' ' + proplist[i] + ' = ' + str(i + 1) + ';\n')
        e.write('}\n\n')
        e.close()
    except Exception as ex:
        print 'asvo,Error : make proto file failed!'
        fmt_str = u'字段名: %s 字段类型: %s '
        print fmt_str % (debug_prop_str, debug_type_str)
        print ex
        os.system('pause')

def make_writefile(proplist, typelist, sheet_name, excel_file_name, need_common_module):
    try:
        py_filename = 'Write_' + sheet_name + '.py'
        if isinstance(py_filename, unicode):
            py_filename = py_filename.encode('utf-8')
        pyw = file(write_py_dir + py_filename, 'w')
        pyw.write('# -*- coding: utf-8 -*-\n')
        pyw.write('import ' + sheet_name + '_pb2\nimport codecs\n\n')
        pyw.write('import CommonMoudle\n\n')
        if need_common_module:
            pyw.write('import commdef_pb2\n\n')
        pyw.write('''
ParsePropertyName = ''
ParseTargetType = ''
ParsePropertyValue = ''\n''')
        pyw.write('def addItem(item, args):\n')
        msg = '\t'
        pyw.write('''\tglobal ParsePropertyName\n\tglobal ParseTargetType\n\tglobal ParsePropertyValue\n''')
        for i in range(len(proplist)):
            type_str = typelist[i]
            if type_str.strip() == '':
                continue
            target_type = calculate_prop_type(type_str)
            set_prop_txt = 'ParsePropertyName = \'' + proplist[i] + '\'\n\t'
            set_prop_txt += 'ParsePropertyValue = ' + 'args[' + str(i) + ']' + '\n\t'
            if check_is_target_type('commdef', type_str):
                enumStr = parseEnumStr(type_str)
                set_prop_txt += 'ParseTargetType = \'' + enumStr +'\' \n\t'
                msg = msg + set_prop_txt
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValuePyExcel(' + enumStr + ',args[' + str(i) + '], True)\n\t'
            #int类型处理
            elif check_is_target_type('int', target_type):
                set_prop_txt += 'ParseTargetType = \'int\' \n\t'
                msg = msg + set_prop_txt
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValuePyExcel(int, args[' + str(i) + '], False)\n\t'
            #string类型处理
            elif check_is_target_type('string', target_type):
                set_prop_txt += 'ParseTargetType = \'str\' \n\t'
                msg = msg + set_prop_txt
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValuePyExcel(str, args[' + str(i) + '], False)\n\t'
            #bool类型处理
            elif check_is_target_type('bool', target_type):
                set_prop_txt += 'ParseTargetType = \'bool\' \n\t'
                msg = msg + set_prop_txt
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValuePyExcel(bool, args[' + str(i) + '], False)\n\t'
            #float类型处理
            elif check_is_target_type('float', target_type):
                set_prop_txt += 'ParseTargetType = \'float\' \n\t'
                msg = msg + set_prop_txt
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValuePyExcel(float, args[' + str(i) + '], False)\n\t'
            #double类型处理
            elif check_is_target_type('double', target_type):
                set_prop_txt += 'ParseTargetType = \'float\' \n\t'
                msg = msg + set_prop_txt
                msg = msg + 'item.' + proplist[i] + ' = ' + 'CommonMoudle.setProtoValuePyExcel(float, args[' + str(i) + '], False)\n\t'
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
    try:
        FirstColVal = line[0]
        addItem(table.tlist.add(), line)
    except Exception,e:
        print e
        print 'addItem Erorr!!! at line %d , table: %s' % (i, table.tname)
        fmt_str = u'第一列值:%s,字段名:%s, 字段类型:%s, 字段值:%s!'
        print fmt_str % (FirstColVal, ParsePropertyName, ParseTargetType, ParsePropertyValue)
        break\n\n'''
        pyw.write(mm)
        pyw.write('\nf = file(\'' + bytes_dir.replace('\\', '\\\\') + sheet_name +'.bytes\', \'wb\')\n')
        pyw.write('f.write(table.SerializeToString())\n')
        pyw.write('f.close()\n')
        pyw.close()
    except Exception as e:
        print 'asvo,Error : make write file failed!'
        print e
        os.system('pause')

original_type_str_list = ['int', 'uint', 'string', 'float', 'bool', 'commdef', 'vector', 'struct']
str_family_list = ['string', 'vector', 'struct']
def calculate_prop_type(type_str):
    try:
        for i in range(len(original_type_str_list)):
            original_type_str = original_type_str_list[i]
            is_type = check_type_by_head(original_type_str, type_str)
            if is_type:
                if check_type_in_str_family(original_type_str):
                    return 'string'
                else:
                    return type_str
                break
    except Exception as e:
        print 'asvo,Error : type-exchage, no such type %s!' % (type_str)
        print e
        os.system('pause')

def check_type_in_str_family(type_str):
    return type_str in str_family_list

def check_prop_type_str(type_str):
    return check_type_by_head('string', type_str) or check_type_by_head('vector', type_str) or check_type_by_head('struct', type_str)

def check_type_by_head(head_str, type_str):
    head_str_len = len(head_str)
    cmp_str = type_str[:head_str_len]
    return head_str == cmp_str

def check_is_target_type(check_str, type_str):
    return type_str.find(check_str) != -1

def export_to_csv(excel_file):
    if isinstance(excel_file, unicode):
        excel_file = excel_file.encode('utf-8')
    print 'export csv file %s' % (excel_file)
    if excelfile[-5:] == '.xlsx':
        import test_openpyxl
        test_openpyxl.xlsx2csv(excel_file, excel_dir, csv_dir)
    elif excelfile[-4:] == '.xls':
        import test_pyExcelrator
        xls_exporter = test_pyExcelrator.batxls2csv()
        xls_exporter.save_xls2csv(excelfile, excel_dir, csv_dir)
    else:
        print 'asvo,Error : Unsupport excel data format! file: %s' % (excel_file)
        os.system('pause')

#生成proto文件，write_xx.py文件
def build_excel_file(xls_filename, is_build_proto_only):
    #处理单个excel文件
    data = open_excel(excel_dir + xls_filename)
    if None == data:
        print 'asvo,Error : open_excel error!'
        os.system('pause')
        return
    sheet_names = data.sheet_names()
    for name in sheet_names:
        #处理每个sheet
        sheet = data.sheet_by_name(name)
        #不足3行的数据不读取
        if sheet.nrows < 3:    #?????? <=? or <?
            continue
        prop_list = sheet.row_values(1)
        type_list = sheet.row_values(2)
        #如果第一个属性名是空，则认为表单为空表单。不处理
        if prop_list[0].strip() == '':
            continue
        needImportCommon = False
        for typeString in type_list:
            if typeString.find('commdef') != -1:
                needImportCommon = True
        make_protofile(prop_list, type_list, name, needImportCommon)
        if not is_build_proto_only:
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


cleanDir(bytes_dir)
cleanDir(write_py_dir) #clean python proto dir
cleanDir(proto_dir) #clean proto output dir
#clean csv dir
#cleanDir(csv_dir)

build_proto_files_only = use_csv_export == '2'
excel_file_list = os.listdir(excel_dir)
for excelfile in excel_file_list:
    print excelfile
    #针对目录下可能并非全都是excel格式的文件的判断
    if excelfile[-5:] == '.xlsx' or excelfile[-4:] == '.xls':
        build_excel_file(excelfile, build_proto_files_only)
        #excel文件导出csv文件
    if use_csv_export == '1':
        export_to_csv(excelfile)

#excute(protogen.exe)
#exceute(write.py)
if not build_proto_files_only:
    pyCmd = 'protoc.exe --python_out=' + write_py_dir + ' --proto_path=' + proto_dir + ' ' + proto_dir + '*.proto' #define py cmd

    pyresult = os.system(pyCmd) #make proto.py files
    if pyresult == 0:
        makeDBP()
    else:
        print pyCmd
        print 'asvo,Error : python cmd excute failed!'

#cleanDir(pythonOutDir) #clean python proto dir
#cleanDir(cppOutDir) #clean cpp proto dir
#清理临时excel目录
cleanDir(excel_dir)

print 'press enter key to continue...'
raw_input()
