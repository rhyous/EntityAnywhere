import { SpaceTitlePipe } from './spacetitle.pipe'

describe('Space Title Pipe', () => {
    it('create an instance', () => {
        const pipe = new SpaceTitlePipe()
        expect(pipe).toBeTruthy()
    })

    it('should handle ABCDEFG', () => {
        // Arrange
        const pipe = new SpaceTitlePipe()

        // Act
        const transformValue = pipe.transform('ABCDEFG')

        // Assert
        expect(transformValue).toEqual('A B C D E F G')
    })

    it('should handle abcdefg', () => {
        // Arrange
        const pipe = new SpaceTitlePipe()

        // Act
        const transformValue = pipe.transform('abcdefg')

        // Assert
        expect(transformValue).toEqual('abcdefg')
    })

    it('should handle AbCdEfG', () => {
        // Arrange
        const pipe = new SpaceTitlePipe()

        // Act
        const transformValue = pipe.transform('AbCdEfG')

        // Assert
        expect(transformValue).toEqual('Ab Cd Ef G')
    })

    it('should handle ABCDefg', () => {
        // Arrange
        const pipe = new SpaceTitlePipe()

        // Act
        const transformValue = pipe.transform('ABCDefg')

        // Assert
        expect(transformValue).toEqual('A B C Defg')
    })

    it('should handle abcdEFG', () => {
        // Arrange
        const pipe = new SpaceTitlePipe()

        // Act
        const transformValue = pipe.transform('abcdEFG')

        // Assert
        expect(transformValue).toEqual('abcd E F G')
    })

})
