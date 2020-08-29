# coding: UTF-8

import io
import sys
import os
import re
import json
import copy
import platform

sys.stdout = io.TextIOWrapper(sys.stdout.buffer,encoding='utf-8')

from PyScript import convert
from PyScript import file

def main():
    config = release_projects_config()
    address = config.get('address', None)
    if not address:
        print('地址不正确: {}'.format(address))
        return
    projects = config.get('projects', [])
    root = os.path.abspath(sys.argv[0])
    root = os.path.split(root)[0]
    for path in projects:
        source = os.path.join(root, path)
        os.chdir(source)
        target = file.to_abs_path(address, path)
        cmd = 'dotnet publish -o {}'.format(target)
        print('发布项目命令: {}'.format(cmd))
        convert.execute_command(cmd)

def release_projects_config():
    address = '/var/wwwroot/YTSCSharpDotNetCore'
    if convert.is_window_system():
        address = 'D:\\wwwroot\\YTSCSharpDotNetCore'
    return file.read_program_config('release.projects.json', {
        'address': address,
        'projects': [
            'Test.ConsoleProgram',
            'YTS.AdminWeb',
            'YTS.AdminWebApi',
        ],
    })

if __name__ == '__main__':
    main()
