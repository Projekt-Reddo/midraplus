// Libs
import * as React from "react";
import { RootStateOrAny, useDispatch, useSelector } from "react-redux";
import LC from "literallycanvas";

// Components
import LeftToolBar from "components/LeftToolBar";
import Cursor from "components/Cursor";

// Store
import { ADD_NOTE, DRAW_SHAPE, INITLC, SEND_MOUSE } from "store/actions";

// Style
import "literallycanvas/lib/css/literallycanvas.css";
import "styles/CanvasBoard.css";
import { OtherTool } from "utils/constant";
import Loading from "./Loading";

interface CanvasBoardProps {}

var timer: any;

const CanvasBoard: React.FC<CanvasBoardProps> = () => {
    // Redux state
    const dispatch = useDispatch();
    const shape = useSelector((state: RootStateOrAny) => state.shape);
    const myShape = useSelector((state: RootStateOrAny) => state.myShape);
    const onlineUsers = useSelector((state: any) => state.onlineUsers);
    const tool = useSelector((state: RootStateOrAny) => state.tool);
    const initLC = useSelector((state: RootStateOrAny) => state.initLC);

    // Handle State
    const [localInitLC, setLocalInitLC] = React.useState<typeof LC>();
    const [firstLoad, setFirstLoad] = React.useState<boolean>(true);

    // Get Change of Canvas
    // Send user's shape to other user
    // Set user's shape to myShape state
    const handleDrawingChange = (lc: any, shape: any) => {
        const lcShapeContainer = lc.getSnapshot(["shapes"]);
        dispatch({
            type: DRAW_SHAPE,
            payload:
                lcShapeContainer.shapes[lcShapeContainer.shapes.length - 1],
        });
    };

    // Create Init of Literally Canvas
    const handleInit = (lc: any) => {
        setLocalInitLC(lc);
        dispatch({ type: INITLC, payload: lc });
        lc.on("shapeSave", (shape: any) => handleDrawingChange(lc, shape));
    };

    const handleCreateNote = (e: React.MouseEvent<HTMLElement>) => {
        dispatch({
            type: ADD_NOTE,
            payload: {
                x: e.clientX,
                y: e.clientY,
                text: "",
                id: `${Date.now()} ${Math.random()}`,
            },
        });
    };

    // Load Shape of the other User
    React.useEffect(() => {
        if (localInitLC && shape !== []) {
            localInitLC.loadSnapshot({
                shapes: [...shape, ...myShape.undoStack],
            });

            if (firstLoad) setFirstLoad(false);
        }
    }, [shape, myShape]);

    

    const getMousePosition = (e: any) => {
        if (onlineUsers.length > 1) {
            dispatch({
                type: SEND_MOUSE,
                payload: {
                    x: e.pageX,
                    y: e.pageY,
                    isMove: true,
                },
            });
            clearTimeout(timer);
            timer = setTimeout(() => {
                dispatch({
                    type: SEND_MOUSE,
                    payload: {
                        x: e.pageX,
                        y: e.pageY,
                        isMove: false,
                    },
                });
            }, 300);
        }
    };

    // Handle mouse wheel
    const onScroll = (e: React.WheelEvent<HTMLDivElement>) => {
        let scale = e.deltaY * 0.001;
        initLC.zoom(scale);
    };

    

    return (
        <>
            {firstLoad && (
                <div className="max-w-full min-h-screen bg-[color:var(--bg)]">
                    <div className="w-full h-screen flex justify-center items-center">
                        <Loading />
                    </div>
                </div>
            )}
            <div
                onMouseMove={getMousePosition}
                onWheelCapture={onScroll}
                onClick={tool === OtherTool ? handleCreateNote : () => {}}
            >
                {/* Cursor */}
                <Cursor />
                {/* Left Toolbar */}
                <LeftToolBar
                    onClick={(e: React.MouseEvent<HTMLElement>) => {
                        e.stopPropagation();
                    }}
                />
                {/* Canvas Board */}
                <LC.LiterallyCanvasReactComponent
                    onInit={handleInit}
                    primaryColor="#fff"
                    backgroundColor="#232222"
                    toolbarPosition="hidden"
                />
            </div>
        </>
    );
};

export default CanvasBoard;
