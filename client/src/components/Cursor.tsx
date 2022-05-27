import * as React from "react";
import { RootStateOrAny, useSelector } from "react-redux";

// Style
import "styles/Cursor.css";

interface CursorProps {}

const Cursor: React.FC<CursorProps> = () => {
    const mouses = useSelector((state: RootStateOrAny) => state.mouse);
    return (
        <>
            {mouses
                .filter((mouse: any) => mouse.isMove === true)
                .map((mouse: any) => (
                    <div key={mouse.userId}>
                        {/* Cursor */}
                        <div
                            className="cursor"
                            style={{
                                left: mouse.x,
                                top: mouse.y,
                            }}
                        ></div>
                        {/* Username */}
                        <div
                            className="cursor-outline text-center text-xs text-white p-1"
                            style={{
                                left: mouse.x + 9,
                                top: mouse.y + 9,
                            }}
                        >
                            {mouse.userName}
                        </div>
                    </div>
                ))}
        </>
    );
};

export default Cursor;
