import os
from fastapi import FastAPI, File, UploadFile
from fastapi.responses import FileResponse
import asyncio

app = FastAPI()

@app.post("/worldmap")
def upload(worldmap: UploadFile = File(...)):
    try:
        contents = worldmap.file.read()
        with open(worldmap.filename, 'wb') as f:
            f.write(contents)
    except Exception:
        return {"message": "There was an error uploading the file"}
    finally:
        worldmap.file.close()

@app.post("/anchordict")
def upload(anchordict: UploadFile = File(...)):
    try:
        contents = anchordict.file.read()
        with open(anchordict.filename, 'wb') as f:
            f.write(contents)
    except Exception:
        return {"message": "There was an error uploading the file"}
    finally:
        anchordict.file.close()

@app.get("/my_session.worldmap")
async def root():
    return FileResponse(path="my_session.worldmap", filename="my_session.worldmap", media_type='application/octet-stream')

@app.get("/motiondata.csv")
async def root():
    return FileResponse(path="motiondata.csv", filename="motiondata.csv", media_type='text/csv')

@app.get("/anchordict.txt")
async def root():
    return FileResponse(path="anchordict.txt", filename="anchordict.txt", media_type='text/csv')

@app.post("/motionlog")
def upload(motionlog: UploadFile = File(...)):
    try:
        contents = motionlog.file.read()
        with open(motionlog.filename, 'ab') as f:
            f.write(contents)
    except Exception:
        return {"message": "There was an error uploading the file"}
    finally:
        motionlog.file.close()




