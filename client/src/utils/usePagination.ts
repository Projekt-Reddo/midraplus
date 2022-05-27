import { useMemo } from "react";

export function UsePagination({
    totalRecords,
    currentPage,
    pageSize,
}: {
    totalRecords: number;
    currentPage: number;
    pageSize: number;
}) {
    const totalPage = useMemo(() => {
        return Math.ceil(totalRecords / pageSize);
    }, [totalRecords, pageSize]);

    const paginationRange = useMemo(() => {
        //Create an array containing numbers from start to end
        const range = (start: any, end: any) => {
            const length = end - start + 1;
            return Array.from({ length }, (_, index) => index + start);
        };

        const numberOfSibling = 2;
        const leftSiblingIndex = Math.max(currentPage - numberOfSibling, 1);
        const rightSiblingIndex = Math.min(
            currentPage + numberOfSibling,
            totalPage
        );

        return range(leftSiblingIndex, rightSiblingIndex);
    }, [currentPage, totalPage]);

    return [totalPage, paginationRange];
}
