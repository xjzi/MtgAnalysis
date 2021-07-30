export default async function FetchJson(url){
  const response = await fetch(url);
  if (response.ok) {
    return await response.json();
  } else {
    console.error(`API responded with status ${response.status}`);
  }
}