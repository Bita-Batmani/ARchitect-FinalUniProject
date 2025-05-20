from flask import Flask, request, jsonify
from ultralytics import YOLO
import cv2
import os

app = Flask(__name__)

# 📌 YOLOv8 model path
MODEL_PATH = "best.pt"
if not os.path.exists(MODEL_PATH):
    raise FileNotFoundError(f"Model file {MODEL_PATH} not found!")

# Load the model
model = YOLO(MODEL_PATH)

# Model classes as trained
class_names = ['Fani 1', 'Fani 2', 'Masjed', 'Library']

# Mapping dictionaries
class_to_building_id = {
    "Fani 1": 1,
    "Fani 2": 2,
    "Masjed": 3,
    "Library": 4
}

class_to_display_name = {
    "Fani 1": "Faculty of Engineering 1",
    "Fani 2": "Faculty of Engineering 2",
    "Masjed": "University Mosque",
    "Library": "Central Library & Research Building"
}

# 📌 File upload folder
UPLOAD_FOLDER = "uploads"
os.makedirs(UPLOAD_FOLDER, exist_ok=True)

@app.route("/")
def home():
    return "🚀 YOLOv8 Flask API is running!"

@app.route("/predict", methods=["POST"])
def predict():
    if "image" not in request.files:
        return jsonify({"error": "No image file provided"}), 400

    # Save the uploaded file
    file = request.files["image"]
    filename = os.path.join(UPLOAD_FOLDER, file.filename)
    file.save(filename)

    # Run inference using YOLOv8
    results = model(filename)

    # Extract bounding boxes and map class names to building details
    output = []
    for result in results:
        for box in result.boxes:
            cls_id = int(box.cls[0].item())
            confidence = float(box.conf[0].item())
            bbox = [float(x) for x in box.xyxy[0].tolist()]

            # Map to known classes
            original_class = class_names[cls_id] if cls_id < len(class_names) else f"Class {cls_id}"
            building_id = class_to_building_id.get(original_class, None)
            display_name = class_to_display_name.get(original_class, original_class)

            output.append({
                "bbox": bbox,
                "class_name": original_class,   # Renamed key to match Unity
                "confidence": confidence,
                "building_id": building_id,
                "display_name": display_name
            })

    return jsonify({"predictions": output})

if __name__ == "__main__":
    print("🔥 Flask server is starting...")
    # Host on 0.0.0.0 so other devices on LAN can access
    app.run(host='0.0.0.0', port=5000, debug=True)
