export default function DataHandler({error, data, render}){
    if(error){
        return <div>Error</div>
      } else if (!data) {
        return <div>Loading...</div>;
      } else {
        return render();
      }
}