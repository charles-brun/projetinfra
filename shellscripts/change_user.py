#!/usr/bin/python
import pathlib
import sys
FileContent = ""
dir=pathlib.Path("MM")

myPath = pathlib.Path(__file__).parent.resolve().parent.resolve().__str__()
myPath += "/services/" + sys.argv[1]
print(myPath)
with open(myPath) as f:
    FileContent = f.read()
if "[USERNAME]" in FileContent:
        FileContent = FileContent.replace("[USERNAME]", sys.argv[2])
with open(myPath, 'w') as f:
    f.write(FileContent)
