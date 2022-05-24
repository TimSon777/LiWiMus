namespace LiWiMus.Web.API.Tests.Tracks

open System
open System.Collections.Generic
open LiWiMus.Web.API
open LiWiMus.Web.API.Shared
open LiWiMus.Web.API.Tests
open Xunit
open System.Net.Http.Json
open FluentAssertions
open Microsoft.AspNetCore.Http.Json
open LiWiMus.Web.API.Tests.JsonOptionsExtensions
open LiWiMus.Web.API.Tests.Tracks.Generators

type CreateTests(
        factory: TestApplicationFactory) =
        let url = RouteConstants.Tracks.Create
        interface IClassFixture<TestApplicationFactory>


                    [<Theory>]
                    [<ClassData(typeof<CreateTestsGeneratorSuccess>)>]
                    member this.``Tests(Tracks): Create => Success``(
                        albumId, name, publishedAt, fileLocation, genreIds, ownerIds, duration) =
                        
                        // Arrange
                        let client = factory.CreateClient()
                
                        let body = Tracks.Create.Request(
                            AlbumId = albumId,
                            Name = name,
                            PublishedAt = publishedAt,
                            FileLocation = fileLocation,
                            GenreIds = genreIds,
                            OwnerIds = ownerIds,
                            Duration = duration
                        )
                        
                        task {
                            
                            //Act
                            let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                            
                            //Assert
                            httpMessage
                                .Should()
                                .BeSuccessful("all params must be valid (see Validator)")
                                |> ignore
                        }

        [<Theory>]
        [<ClassData(typeof<CreateTestsGeneratorFailure>)>]
        member this.``Tests(Tracks): Create => Failure (bad request)``(
            albumId, name, publishedAt, fileLocation, genreIds, ownerIds, duration) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Tracks.Create.Request(
                AlbumId = albumId,
                Name = name,
                PublishedAt = publishedAt,
                FileLocation = fileLocation,
                GenreIds = genreIds,
                OwnerIds = ownerIds,
                Duration = duration
            )
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("request with invalid params must return error status code")
                    |> ignore
            }
            
        [<Theory>]
        [<InlineData(225000, 220000, 220000)>]
        [<InlineData(220000, 225000, 220000)>]
        [<InlineData(220000, 220000, 225000)>]
        member this.``Tests(Tracks): Create => Failure (unprocessable entity)``(
            albumId, genreId, ownerId) =
            
            // Arrange
            let client = factory.CreateClient()
    
            let body = Tracks.Create.Request(
                AlbumId = albumId,
                Name = "TestTrackName",
                PublishedAt = DateOnly(),
                FileLocation = "Location",
                GenreIds = List<int>([genreId]),
                OwnerIds = List<int>([ownerId]),
                Duration = 99
            )
            
            task {
                
                //Act
                let! httpMessage = client.PostAsJsonAsync(url, body, JsonOptions().WithDateTimeOnly())
                
                //Assert
                httpMessage
                    .Should()
                    .HaveClientError("album / genre / artist doesn't exist")
                    |> ignore
            }        