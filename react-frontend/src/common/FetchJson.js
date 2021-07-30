export default async function FetchJson(url){
  const response = await fetch('http://api:1337' + url);
  if (response.ok) {
    return await response.json();
  } else {
    console.error(`API responded with status ${response.status}`);
  }
}