export default function Error() {
  return (
    <div className="flex flex-col">
      <h1 className="text-5xl font-bold text-center mt-10">Oooops!</h1>
      <div className="text-2xl text-center mt-10">
        An internal error occured!
      </div>
      <div className="text-center">
        If the problem persists, contact your administrator.
      </div>
    </div>
  );
}
