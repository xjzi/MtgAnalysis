import FetchJson from "./FetchJson";

export default async function GetImage(card, format){
  const url = new URL('https://api.scryfall.com/cards/named');
  url.searchParams.append('exact', card);
  const json = await FetchJson(url);
  return json.image_uris?.[format] ?? json.card_faces[0]?.image_uris[format];
}