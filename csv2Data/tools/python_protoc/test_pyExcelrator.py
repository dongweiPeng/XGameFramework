#! /usr/bin/env python
#coding=utf-8
#writer:parming    Email：ming5536@163.com
#pyExcelerator只支持xls格式的表
from pyExcelerator import *
import sys
import glob
import codecs

class batxls2csv:
    def __init__(self):
                pass
    def savecsv1(self,arg):
        matrixgolb = []
        for sheet_name, values in parse_xls(arg, 'cp1251'): # parse_xls(arg) -- default encoding
            matrix = [[]]
            for row_idx, col_idx in sorted(values.keys()):
                #print row_idx,col_idx
                #print matrix
                v = values[(row_idx, col_idx)]
                if isinstance(v, unicode):
                    #v = v.encode('cp866', 'backslashreplace')
                    v = v.encode('utf-8')
                else:
                    v = str(v)
                last_row, last_col = len(matrix), len(matrix[-1])
                #下一行修改过
                while last_row <=row_idx:
                    matrix.extend([[]])
                    last_row = len(matrix)

                while last_col < col_idx:
                    matrix[-1].extend([''])
                    last_col = len(matrix[-1])

                matrix[-1].extend([v])
            for row in matrix:
                 csv_row = ','.join(row)
                 matrixgolb.append(csv_row)
        return matrixgolb

    def save_xls2csv(self, filename, file_dir, csv_path):
        try:
            file_path = file_dir + filename
            matrixgolb=self.savecsv1(file_path)
            namecsv=filename[:-4]+'.csv'
            file_object = open(csv_path + namecsv, 'w+')
            file_object.write(codecs.BOM_UTF8)
            for item in matrixgolb:
                file_object.write(item)
                file_object.write('\n')
                #print item
            file_object.close()
            print 'export csv file: %s succ~' % (namecsv)
        except Exception as e:
            print 'export excel file %s to csv failed!' % (filename)
            print(e)
