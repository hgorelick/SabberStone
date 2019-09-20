import os
import subprocess
import sys
from sabberstats import *

navigate = 'C://Users//hgore//OneDrive - Fordham University//Documents//Fordham//Thesis//SabberStone//' \
           'core-extensions//SabberStoneCoreAi//bin//Release//netcoreapp2.0'

os.chdir(navigate)

command = "dotnet SabberStoneCoreAi.dll"

p = subprocess.Popen(command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.STDOUT)

while True:
    state = []
    out = p.stdout.readline().decode('utf-8')
    if 'Turn' in out:
        line = p.stdout.readline().decode('utf-8')
        while line != "------------":
            state.append(line)
            line = p.stdout.readline().decode('utf-8')
        stats = SabberStats(out, state)

    if out == '' and p.poll() is not None:
        break
    if out != '':
        if '|' in out:
            if 'Turn' in out:
                out.strip().strip('|')[-1]
        sys.stdout.write(out)
        sys.stdout.flush()
