import os
import re
import shutil

# configuration
inputDir = "."
songsDir = "Assets\Songs"
outputDir = ".."

# prepare output
songsOutputDir = os.path.join(outputDir, songsDir)
if not os.path.isdir(songsOutputDir):
    os.makedirs(songsOutputDir)

# process all input files
songCollection = []

def CopyContents(source: str, dest):
    f_source = open(source, encoding="utf-8", errors="ignore")
    lines = f_source.readlines()
    f_source.close()

    dest.writelines(lines)

def InitSongFile(fileName: str, song: dict):
    f = open(fileName, "w", encoding="utf-8")

    CopyContents("songFileHeader.html", f)

    f.write("<h1>" + song["number"] + ": " + song["name"] + "</h1>\n")

    additions = []

    if song["author"]:
        additions.append(song["author"]);

    if song["source"]:
        additions.append("<i>" + song["source"] + "</i>")

    addLine = ", ".join(additions)
    if addLine:
        f.write("<h3>" + addLine + "</h3>\n")

    return f

def FinaliseSongFile(file):
    if file == None:
        return

    CopyContents("songFileFooter.html", file)

    file.close()

def ProcessOneFile(fileName: str):
    f_in = open(fileName, "r", encoding="utf-8", errors="ignore")
    lines = f_in.readlines()

    f_out = None

    newSongPattern = re.compile("\\\\newsong{(.*)}")
    newVersePattern = re.compile("^(\d+)\.\s*(.*)$")
    translationPattern = re.compile("\\\\translation{(.*)}")

    # process each line
    for line in lines:
        # remove whitespace
        line = line.strip()

        # skip comments
        if len(line) > 0 and line[0] == "#":
            continue

        # is it a newsong?
        sr = re.search(newSongPattern, line)
        if (sr):
            #print(sr.group(1))

            # parse line
            arguments = sr.group(1).split("|")
            number = arguments[0]
            name = arguments[1]
            author = arguments[2] if len(arguments) > 2 else ""
            source = arguments[3] if len(arguments) > 3 else ""

            # aggreate all information
            fn_out = number + ".html"
            song = {"number": number, "name": name, "author": author, "source": source, "file": fn_out}

            # add to collection
            songCollection.append(song)

            # close old file (if any)
            FinaliseSongFile(f_out)

            # open new file
            f_out = InitSongFile(os.path.join(outputDir, songsDir, fn_out), song)

            continue

        # shall add an empty line
        addLine = False
        if ("{REF}" in line) or ("{ODP}" in line):
            addLine = True

        newVerseFound = re.search(newVersePattern, line)
        if newVerseFound:
            addLine = True

        if addLine:
            f_out.write("<br>\n")

        # replacements
        if newVerseFound:
            line = "<b>{}.</b> {}".format(newVerseFound.group(1), newVerseFound.group(2))

        line = line.replace("{REF}", "<b>REF: </b>").replace("{ODP}", "<b>ODP: </b>")

        line = re.sub(translationPattern, lambda m : "<i>Překlad: " + m.group(1) + "</i>", line)

        # write to output
        if f_out != None:
            f_out.write(line + "<br>\n")

    FinaliseSongFile(f_out)

    f_in.close()

inputFiles = [f for f in os.listdir(inputDir) if (".txt" in f) and os.path.isfile(os.path.join(inputDir, f))]
for fileName in inputFiles:
    ProcessOneFile(os.path.join(inputDir, fileName))

# write search function
f_out = open(os.path.join(outputDir, "Search.gen.cs"), "w", encoding="utf-8")

CopyContents("searchFileHeader.cs", f_out)

for song in songCollection:
    f_out.write("\t\t\tallSongs.Add(new SongData(\"{}\", \"{}\"));\n".format(song["number"], song["name"]))

CopyContents("searchFileFooter.cs", f_out)