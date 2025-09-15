import React from "react";
import chevronLeft from "../../assets/chevron-left.svg";
import chevronRight from "../../assets/chevron-right.svg";

export default function Pagination({ currentPage, totalPages, onPageChange }) {
    if (totalPages <= 1) return null;

    const pages = [];
    for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
    }

    return (
        <div className="d-flex justify-content-center mt-4">
            <ul className="pagination">
                <li className={`page-item ${currentPage === 1 ? "disabled" : ""}`}>
                    <button
                        className="page-link border-0 bg-transparent p-0"
                        onClick={() => onPageChange(currentPage - 1)}
                        disabled={currentPage === 1}
                        style={{ width: '32px', height: '32px' }}
                    >
                        <img src={chevronLeft} alt="Prev" />
                    </button>
                </li>

                {pages.map((page) => (
                    <li
                        key={page}
                        className={`page-item ${page === currentPage ? "active" : ""}`}
                    >
                        <button
                            className="page-link mx-1 border-0"
                            onClick={() => onPageChange(page)}
                            style={{
                                fontSize: '1.2rem',
                                width: '2.5rem',
                                height: '2.5rem',
                                borderRadius: '50%',
                                color: page === currentPage ? '#a3a19eff' : '#a3a19eff',
                                backgroundColor: page === currentPage ? '#263238' : '#ececec'
                            }}
                        >
                            {page}
                        </button>
                    </li>
                ))}

                <li className={`page-item ${currentPage === totalPages ? "disabled" : ""}`}>
                    <button
                        className="page-link border-0 bg-transparent p-0"
                        onClick={() => onPageChange(currentPage + 1)}
                        disabled={currentPage === totalPages}
                        style={{ width: '32px', height: '32px' }}
                    >
                        <img src={chevronRight} alt="Next" />
                    </button>
                </li>
            </ul>
        </div>
    );
}
