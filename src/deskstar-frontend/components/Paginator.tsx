export default function Paginator({
  n,
  total,
  currentPage,
  onChange,
}: {
  n: number;
  total: number;
  currentPage: number;
  onChange: Function;
}) {
  return (
    <div className="btn-group">
      <button
        className={`btn btn-success ${currentPage === 0 ? "btn-disabled" : ""}`}
        onClick={() => onChange(currentPage - 1)}
      >
        «
      </button>
      <button className="btn btn-success">Page {currentPage + 1}</button>
      <button
        className={`btn btn-success ${
          (currentPage + 1) * n > total - 1 ? "btn-disabled" : ""
        }`}
        onClick={() => onChange(currentPage + 1)}
      >
        »
      </button>
    </div>
  );
}
