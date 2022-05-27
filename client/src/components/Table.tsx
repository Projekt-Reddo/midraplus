import * as React from "react";
import "styles/Table.css";

interface TableProps {
    className?: string;
    style?: object;
    columns: string[];
    data: object[];
    onClick?: () => void;
    isSelectable?: boolean;
    isVeryLong?: boolean;
}

/**
 * Table
 * @param {string} className the className pass to the outer most div
 * @param {object} style the styles pass to the outer most div
 * @param {array of strings} columns the columns want to render
 * @param {array of objects} data the data pass to the table corresponding with columns
 * @param {function} onClick the onClick callback on table row
 * @param {boolean} isSelectable the boolean for setting the cursor when hovering on the table
 * @param {boolean} isVeryLong the boolean for freezing the table header
 */
const Table: React.FC<TableProps> = ({
    className = "",
    style = {},
    columns = [],
    data = [],
    onClick = (index: number) => {},
    isSelectable = false,
    isVeryLong = false,
}) => {
    // Generating unique keys
    const generateKey = (pre: any) => {
        return `${pre}_${new Date().getTime()}`;
    };
    // Freezing the table when there are more than 5 cols
    const isFreeze = columns.length > 5 ? true : false;
    // Append more styles to the outer most div when Freezing the table
    // const defaultOverflow = { overflowX: "scroll" };
    return (
        <div
            className={`${className} `}
            style={
                isFreeze
                    ? { ...{ overflowX: "scroll" }, ...style }
                    : { ...style }
            }
        >
            <table
                /* Add class sticky-col when the table is freezing cols*/
                className={`datatable w-100 h-100 text-center ${
                    isFreeze
                        ? "sticky-col"
                        : `${isVeryLong ? "sticky-head" : ""}`
                } `}
            >
                <thead>
                    {data.length > 0 && (
                        <tr>
                            {columns.map((col, colIndex) => (
                                <th key={generateKey(col)}>{col}</th>
                            ))}
                        </tr>
                    )}
                </thead>
                <tbody>
                    {data.length > 0 &&
                        data.map((obj: any, objIndex) => (
                            <tr
                                key={objIndex + "tr"}
                                onClick={() => onClick(objIndex)}
                                className={isSelectable ? "selectable" : ""}
                            >
                                {Object.keys(obj).map((key, keyIndex) => (
                                    <td key={generateKey(key) + obj[key]}>
                                        <span
                                            className="mobile"
                                            key={
                                                generateKey(key) +
                                                "spantitle" +
                                                obj[key]
                                            }
                                        >
                                            {columns[keyIndex] === ""
                                                ? ""
                                                : columns[keyIndex] + " : "}
                                        </span>
                                        <span
                                            key={
                                                generateKey(key) +
                                                "spanvalue" +
                                                obj[key]
                                            }
                                        >
                                            {obj[key]}
                                        </span>
                                    </td>
                                ))}
                            </tr>
                        ))}
                </tbody>
            </table>
        </div>
    );
};

export default Table;
