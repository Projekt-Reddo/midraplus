export const API = process.env.REACT_APP_API_URL;
export const DefaultDay = "0001-01-01T00:00:00Z";
export const GOOGLE_CLIENT_ID = process.env.REACT_APP_GOOGLE_CLIENT_ID || "";
export const GOOGLE_AUTH_CALLBACK_URL = `${API}/api/auth/google`;
export const OtherTool = "Tool";
export const Pencil = "Pencil";
export const Eraser = "Eraser";
export const Text = "Text";
export const Pan = "Pan";
export const ColorCode = [
    "#E61225",
    "#AB238A",
    "#D30079",
    "#F6620D",
    "#5A2C90",
    "#C19E67",
    "#FFC015",
    "#0168BF",
    "#B6B7B6",
    "#03A557",
    "#33CDFF",
    "#fff",
];
export const StrokeWidth = [
    { width: 9, size: "eyeXL" },
    { width: 5, size: "eyeL" },
    { width: 3, size: "eyeM" },
    { width: 2, size: "eyeS" },
];
