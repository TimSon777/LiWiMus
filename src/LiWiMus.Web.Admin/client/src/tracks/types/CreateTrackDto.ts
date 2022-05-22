export type CreateTrackDto ={
    albumId: number,
    name: string,
    publishedAt: string,
    fileLocation: string,
    genreIds: number[],
    ownerIds: number[],
    duration: number
}