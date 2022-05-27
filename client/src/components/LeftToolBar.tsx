// Libs
import * as React from "react";
import { RootStateOrAny, useDispatch, useSelector } from "react-redux";
import LC from "literallycanvas";

// Components
import Icon from "components/Icon";

//Style
import "styles/LeftToolBar.css";
import {
    OtherTool,
    Pencil,
    Text,
    Eraser,
    Pan,
    ColorCode,
    StrokeWidth,
} from "utils/constant";

//Store
import { CLEAR_ALL, REDO, UNDO } from "store/actions";

interface LeftToolBarProps {
    onClick: (e: React.MouseEvent<HTMLElement>) => void;
}

const doNothing = () => {};

var watingClick: any = null;
var lastClick = 0;

const LeftToolBar: React.FC<LeftToolBarProps> = ({ onClick }) => {
    // Global State
    const dispatch = useDispatch();
    const initLC = useSelector((state: RootStateOrAny) => state.initLC);
    const shape = useSelector((state: RootStateOrAny) => state.shape);

    // Handle State
    const [showBrushOption, setShowBrushOption] = React.useState(false);
    const [clearAllOption, setClearrAllOption] = React.useState(false);
    const [showOtherToolOption, setShowOtherToolOption] = React.useState(false);
    const [isSelect, setIsSelect] = React.useState(1);
    const [colorSelect, setColorSelect] = React.useState("#fff");
    const [strokeWidthSelect, setStrokeWidthSelect] = React.useState(5);

    // State click outside
    const wrapperRef = React.useRef(null);
    useOutsideAlerter(wrapperRef, setShowBrushOption);

    const wrapperRef2 = React.useRef(null);
    useOutsideAlerter(wrapperRef2, setClearrAllOption);

    // Funtions Handle Draw Canvas
    // Select Tool
    const handleSelectTool = (toolName: string) => {
        initLC.setTool(new LC.tools[toolName](initLC));

        dispatch({
            type: toolName,
        });

        if (toolName === Pencil) {
            initLC.tool.strokeWidth = strokeWidthSelect;
        }
        if (toolName === Eraser) {
            initLC.tool.strokeWidth = 30;
        }
    };

    // Select stroke width for Brush
    const handleSelectToolStrokeWidth = (strokeWidth: number) => {
        setStrokeWidthSelect(strokeWidth);
        initLC.tool.strokeWidth = strokeWidth;
    };

    // Select color for Brush and Text tool
    const handleSelectToolColor = (colorCode: string) => {
        initLC.setColor("primary", colorCode);
    };

    // Clear canvas
    const handleClear = (lc: any) => {
        dispatch({
            type: CLEAR_ALL,
        });
    };

    // Undo canvas
    const handleUndo = () => {
        dispatch({
            type: UNDO,
        });
    };

    // Redo canvas
    const handleRedo = () => {
        dispatch({
            type: REDO,
        });
    };

    const triggerDownload = (imgURI: string) => {
        var evt = new MouseEvent("click", {
            view: window,
            bubbles: false,
            cancelable: true,
        });

        var a = document.createElement("a");
        a.setAttribute("download", `${Date.now()}` + ".png");
        a.setAttribute("href", imgURI);
        a.setAttribute("target", "_blank");
        a.setAttribute("preventDefault", "true");
        a.dispatchEvent(evt);
    };

    // Export Image
    const handleExportImage = () => {
        const image = initLC.getImage().toDataURL();
        triggerDownload(image);
    };

    // Handle active Button was selected
    const handleActiveButtonSelect = (buttonCode: number) => {
        if (buttonCode === 6) {
            return;
        }
        setIsSelect(buttonCode);
    };

    // Toolbar Const Variable
    const dToolbars = [
        {
            id: 1,
            iconName: "pen",
            doubleClickFunc: setShowBrushOption,
            showState: showBrushOption,
            toolName: Pencil,
        },
        {
            id: 2,
            iconName: "eraser",
            doubleClickFunc: setClearrAllOption,
            showState: clearAllOption,
            toolName: Eraser,
        },
    ];

    const toolbars = [
        { id: 3, iconName: "font", toolbarFunc: doNothing, toolName: Text },
        {
            id: 4,
            iconName: "sticky-note",
            toolbarFunc: doNothing,
            toolName: OtherTool,
        },
        {
            id: 5,
            iconName: "up-down-left-right",
            toolbarFunc: doNothing,
            toolName: Pan,
        },
    ];

    const oToolbars = [
        { id: 6, iconName: "undo", toolbarFunc: handleUndo },
        { id: 7, iconName: "redo", toolbarFunc: handleRedo },
        {
            id: 8,
            iconName: "share-square",
            toolbarFunc: handleExportImage,
        },
    ];

    return (
        <>
            <div
                className="app-shadow leftToolBar absolute grid grid-cols-1 gap-5 overflow-y-hidden content-center h-[30rem] w-14 z-10"
                onClick={onClick}
            >
                {/* Tools */}
                {/* Brush, Eraser */}
                {dToolbars.map((toolbar) => (
                    <div
                        key={toolbar.id}
                        className="icon flex"
                        style={
                            toolbar.iconName === "pen"
                                ? { color: colorSelect }
                                : {}
                        }
                        onClick={(e) => {
                            if (
                                lastClick &&
                                e.timeStamp - lastClick < 250 &&
                                watingClick
                            ) {
                                lastClick = 0;
                                clearTimeout(watingClick);
                                toolbar.doubleClickFunc(!toolbar.showState);
                                watingClick = null;
                            } else {
                                lastClick = e.timeStamp;
                                watingClick = setTimeout(() => {
                                    watingClick = null;
                                    handleActiveButtonSelect(toolbar.id);
                                    handleSelectTool(toolbar.toolName);
                                }, 251);
                            }
                        }}
                    >
                        <div
                            className={` ${
                                isSelect === toolbar.id ? "whiteLine" : "line"
                            }`}
                        />
                        <div className="text-center self-center w-full">
                            <Icon
                                icon={toolbar.iconName}
                                style={{ fontSize: "1.5rem" }}
                            />
                        </div>
                    </div>
                ))}
                {/* Text, Note, Pan */}
                {toolbars.map((toolbar) => (
                    <div
                        key={toolbar.id}
                        className="icon flex"
                        style={
                            toolbar.iconName === "font"
                                ? { color: colorSelect }
                                : {}
                        }
                        onClick={() => {
                            toolbar.toolbarFunc();
                            handleActiveButtonSelect(toolbar.id);
                            if (toolbar.toolName !== "") {
                                handleSelectTool(toolbar.toolName);
                            }
                        }}
                    >
                        <div
                            className={` ${
                                isSelect === toolbar.id ? "whiteLine" : "line"
                            }`}
                        />
                        <div className="text-center self-center w-full">
                            <Icon
                                icon={toolbar.iconName}
                                style={{ fontSize: "1.5rem" }}
                            />
                        </div>
                    </div>
                ))}
                {/* More Option */}
                <div
                    className="icon flex"
                    onClick={() => setShowOtherToolOption(!showOtherToolOption)}
                >
                    <div className="text-center self-center w-full">
                        <Icon
                            icon="ellipsis-h"
                            style={{ fontSize: "1.5rem" }}
                        />
                    </div>
                </div>
            </div>
            {/* Brush Option Board */}
            <div
                className={` ${
                    showBrushOption
                        ? "app-shadow brushOptionBoard absolute justify-center flex h-44 w-52 z-10"
                        : "brushOptionBoardHide"
                }`}
                ref={wrapperRef}
                onClick={onClick}
            >
                {/* Stroke Options */}
                <div className="grid grid-cols-1 content-center gap-4">
                    {StrokeWidth.map((stroke) => (
                        <div
                            key={stroke.width}
                            className={stroke.size}
                            style={
                                strokeWidthSelect === stroke.width
                                    ? { opacity: 1 }
                                    : {}
                            }
                            onClick={() => {
                                handleSelectToolStrokeWidth(stroke.width);
                            }}
                        />
                    ))}
                </div>
                {/* Vertical White Line */}
                <div className="py-4 mx-4">
                    <div className="verticalLine" />
                </div>
                {/* Color Options */}
                <div className="grid grid-cols-3 content-center gap-4">
                    {ColorCode.map((color) => (
                        <div
                            key={color}
                            style={{
                                backgroundColor: color,
                            }}
                            className="dot"
                            onClick={() => {
                                handleSelectToolColor(color);
                                setColorSelect(color);
                            }}
                        />
                    ))}
                </div>
            </div>
            {/* Clear all Option Board */}
            <div
                className={` ${
                    clearAllOption
                        ? "app-shadow clearAllBoard absolute justify-center flex z-10"
                        : "clearAllBoardHide"
                }`}
                ref={wrapperRef2}
                onClick={onClick}
            >
                <div className="flex justify-center items-center px-3">
                    <Icon
                        icon="trash"
                        size="xl"
                        className="mr-2"
                        onClick={() => {
                            handleClear(initLC);
                        }}
                    />
                    <p>Clear Canvas</p>
                </div>
            </div>
            {/* Undo, Redo, Export */}
            <div
                className={` ${
                    showOtherToolOption
                        ? "app-shadow otherToolOption absolute grid grid-cols-2 content-center h-[7rem] w-[7rem] px-2 z-10"
                        : "otherToolOptionHide"
                }`}
                onClick={onClick}
            >
                {oToolbars.map((toolbar) => (
                    <div
                        key={toolbar.id}
                        className="icon flex"
                        onClick={toolbar.toolbarFunc}
                    >
                        <div className="text-center self-center w-full">
                            <Icon
                                icon={toolbar.iconName}
                                style={{ fontSize: "1.5rem" }}
                            />
                        </div>
                    </div>
                ))}
            </div>
        </>
    );
};

export default LeftToolBar;

// Handle click outside
export function useOutsideAlerter(ref: any, setShowOptionBoard: any) {
    React.useEffect(() => {
        /**
         * Set listData to null
         */
        function handleClickOutside(event: any) {
            if (ref.current && !ref.current.contains(event.target)) {
                setShowOptionBoard(false);
            }
        }

        // Bind the event listener
        document.addEventListener("mousedown", handleClickOutside);
        return () => {
            // Unbind the event listener on clean up
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [ref]);
}
