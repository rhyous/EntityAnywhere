import { Queue } from './queue'

describe('queue', () => {
    it('should queue a number of of items and return them in the correct order', () => {
        const queue: any = new Queue<string>()
        const strings = ['Hello', 'World', 'From', 'The', 'Queue']

        strings.forEach(x => {
            queue.queue(x)
        })

        for (let i = 0; i < queue.count(); i++) {
            expect(strings[i]).toEqual(queue.dequeue())
        }
    })

    it('should return the correct amount of elements', ()  => {
        const queue = new Queue<string>()
        const strings = ['Hello', 'World', 'From', 'The', 'Queue']

        strings.forEach(x => {
            queue.queue(x)
        })

        expect(queue.count()).toEqual(5)
    })
})

