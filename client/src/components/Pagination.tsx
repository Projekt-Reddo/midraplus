import Icon from "components/Icon";
import { UsePagination } from "utils/usePagination";
import "styles/Pagination.css";

/**
 * Handle pagination for a certain data set
 * @param {*} totalRecords The total amount of records of a data set
 * @param {*} currentPage The current page
 * @param {*} pageSize The amount of data to display per page
 * @param {*} onPageChange A function to tell the pagination component what to do when the user clicks on the pagination button
 * @returns
 */
const Pagination = ({
    totalRecords,
    currentPage,
    pageSize,
    onPageChange,
}: {
    totalRecords: number;
    currentPage: number;
    pageSize: number;
    onPageChange: any;
}) => {
    const [totalPage, paginationRange] = UsePagination({
        totalRecords,
        currentPage,
        pageSize,
    });

    if (totalRecords === undefined || totalRecords === 0 || totalPage === 1) {
        return <></>;
    }

    return (
        <>
            <div className="flex justify-center items-center">
                <nav className={`b-radius-6 pagination_Container mb-8`}>
                    {/* Go to first page and go to previous page buttons */}
                    {currentPage !== 1 && (
                        <>
                            <button
                                onClick={() => onPageChange(1)}
                                className="pagination_Button"
                            >
                                <Icon icon="angle-double-left" />
                            </button>
                            <button
                                onClick={() => onPageChange(currentPage - 1)}
                                className="pagination_Button"
                            >
                                <Icon icon="angle-left" />
                            </button>
                        </>
                    )}
                    {/* Page number buttons */}
                    {paginationRange instanceof Array
                        ? paginationRange.map((pageNumber: number) => {
                              if (pageNumber === 1 && currentPage === 1) {
                                  return (
                                      <button
                                          className={
                                              currentPage === pageNumber
                                                  ? "pagination_Button" +
                                                    " " +
                                                    "pagination_Button_Active"
                                                  : "pagination_Button"
                                          }
                                          style={{ marginLeft: "0.5rem" }}
                                          onClick={() =>
                                              onPageChange(pageNumber)
                                          }
                                          key={pageNumber}
                                      >
                                          {pageNumber}
                                      </button>
                                  );
                              }
                              if (
                                  pageNumber === totalPage &&
                                  currentPage === totalPage
                              ) {
                                  return (
                                      <button
                                          className={
                                              currentPage === pageNumber
                                                  ? "pagination_Button" +
                                                    " " +
                                                    "pagination_Button_Active"
                                                  : "pagination_Button"
                                          }
                                          style={{ marginRight: "0.5rem" }}
                                          onClick={() =>
                                              onPageChange(pageNumber)
                                          }
                                          key={pageNumber}
                                      >
                                          {pageNumber}
                                      </button>
                                  );
                              }
                              return (
                                  <button
                                      className={
                                          currentPage === pageNumber
                                              ? "pagination_Button" +
                                                " " +
                                                "pagination_Button_Active"
                                              : "pagination_Button"
                                      }
                                      onClick={() => onPageChange(pageNumber)}
                                      key={pageNumber}
                                  >
                                      {pageNumber}
                                  </button>
                              );
                          })
                        : ""}
                    {/* Go to next page and last page buttons */}
                    {currentPage !== totalPage && (
                        <>
                            <button
                                onClick={() => onPageChange(currentPage + 1)}
                                className="pagination_Button"
                            >
                                <Icon icon="angle-right" />
                            </button>
                            <button
                                onClick={() => onPageChange(totalPage)}
                                className="pagination_Button"
                            >
                                <Icon icon="angle-double-right" />
                            </button>
                        </>
                    )}
                </nav>
            </div>
        </>
    );
};

export default Pagination;
