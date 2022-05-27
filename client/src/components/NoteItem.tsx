import * as React from "react";
import { useDispatch } from "react-redux";
import { DELETE_NOTE, UPDATE_NOTE } from "store/actions";
import Icon from "./Icon";
import "styles/Notes.css";
import ContentEditable from "react-contenteditable";
import { useOutsideAlerter } from "utils/useOutsideAlerter";

interface NoteItemProps {
    note: Note;
}

const NoteItem: React.FC<NoteItemProps> = ({ note: { x, y, id, text } }) => {
    const dispatch = useDispatch();

    const contentEditable = React.useRef<HTMLDivElement>(null);
    const ref = React.useRef<HTMLDivElement>(null);

    const [isOutside, setIsOutside] = React.useState(false);

    const handleClickOutside = (isOutsideClick: boolean) => {
        setIsOutside(isOutsideClick);

        if (isOutsideClick) {
            contentEditable.current?.blur();
        }
    };

    useOutsideAlerter(ref, handleClickOutside);

    const handleChange = (e: any) => {
        dispatch({
            type: UPDATE_NOTE,
            payload: {
                id,
                text: e.target.value,
            },
        });
    };

    return (
        <div
            className={`absolute`}
            style={{
                left: x,
                top: y,
            }}
            ref={ref}
        >
            {!isOutside && (
                <span
                    className="absolute top-0 right-0 inline-flex items-center justify-center p-1 text-xs leading-none text-red-100 transform translate-x-1/2 -translate-y-1/2 bg-red-600 rounded-full cursor-pointer"
                    onClick={() => {
                        dispatch({
                            type: DELETE_NOTE,
                            payload: id,
                        });
                    }}
                >
                    <Icon icon="times" />
                </span>
            )}

            <ContentEditable
                innerRef={contentEditable}
                html={text} // innerHTML of the editable div
                disabled={false} // use true to disable editing
                onChange={handleChange} // handle innerHTML change
                // tagName="article" // Use a custom HTML tag (uses a div by default)
                className="note-input w-100 h-100 p-3 rounded note-item"
            />
        </div>
    );
};

export default NoteItem;
