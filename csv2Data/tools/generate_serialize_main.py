#-*- coding:utf-8 -*-
import codecs
import os

target_apth = 'ProtoCSFile'

def find_class_name_u(file_name_u):
    match_str_pre = 'partial class'
    match_str = ' : global::ProtoBuf.IExtensible'
    result = ''
    rder = file(target_apth + '\\' + file_name_u, 'r')
    lines = rder.readlines()
    rder.close()
    for line in lines:
        find_idx = line.find(match_str_pre)
        if find_idx != -1:
            later_str = line[find_idx + len(match_str_pre):-len(match_str)]
            later_str = later_str.strip()
          #  print later_str
            result = later_str
            break
    return result

mm = 'var model = TypeModel.Create();\n\t\t'
file_list_x = os.listdir(target_apth)
for fileX in file_list_x:
    if fileX[-3:] != '.cs':
        print 'No cs file: %s' % (fileX)
        continue
    if fileX == 'ParticalClass.cs' or fileX == 'commdef.cs':
        print 'Preserved class: %s' % (fileX)
        continue
    table_class_table_name = find_class_name_u(fileX)
    if table_class_table_name == '':
        print 'No match table: %s' % (fileX)
        continue
    match_table_str = 'Table'
    table_class_name = table_class_table_name[:-len(match_table_str)]
    mm += 'model.Add(typeof(' + table_class_table_name + '), true);\n\t\t'
    mm += 'model.Add(typeof(' + table_class_name + '), true);\n\t\t'


mm += 'model.Compile("WdjDTOSerializer", "WdjDTOSerializer.dll");\n'

head_str = '''
using dbc;
using ProtoBuf.Meta;
namespace dbc
{
	public class Program
	{
		static void Main()
		{\n\t\t'''
head_str += mm
head_str += '''
        }
	}
}'''

wr = file('Program.cs', 'w')
wr.write(head_str)
wr.close()

#raw_input()