from fastapi import FastAPI, UploadFile, File
from pydantic import BaseModel
from PIL import Image
import io

from app.face_model import predict_face
from app.spam_model import predict_spam

app = FastAPI()

class TextRequest(BaseModel):
    text: str

@app.post("/predict/face")
async def detect_face(image: UploadFile = File(...)):
    img_bytes = await image.read()
    image_pil = Image.open(io.BytesIO(img_bytes)).convert("RGB")
    result = predict_face(image_pil)
    return {"face_detected": result}

@app.post("/predict/spam")
async def detect_spam(text_input: TextRequest):
    result = predict_spam(text_input.text)
    return {"is_spam": result}
