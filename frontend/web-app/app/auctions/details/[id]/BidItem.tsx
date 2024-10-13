import { numberWithCommas } from '@/app/lib/numberWithComma';
import { Bid } from '@/types';
import { format } from 'date-fns';
import React from 'react'

type Props = {
    bid: Bid
}

export default function BidItem({bid}: Props) {

    function getBidInfo() {
        let bgColor='';
        let text='';
        switch (bid.bidStatus) {
            case 'Accepted': 
                bgColor='bg-green-200'
                text='Bid accept'
                break;
            case 'AcceptedBelowReserve':
                bgColor='bg-amber-500'
                text='Reserve not met'
                break;
            case 'TooLow':
                bgColor= 'bg-red-200'
                text= 'Bid was too low'
                break;
            default:
                bgColor= 'bg-red-200'
                text= 'Bid placed after auction finished'
                break;
        }
        return {bgColor, text}
    }
  return (
    <div className={`
        border-gray-300 border-2 px-3 py-2 rounded-lg
        flex justify-between items-center mb-2
        ${getBidInfo().bgColor}
    `}>
        <div className='flex flex-col'>
            <span>Bidder: {bid.bidder}</span>
            {/* download:npm install date-fns để điều chỉnh cái time theo format, không thì nó sẽ ở dạng như thế này: 2024-10-13T04:05:33.866Z */}
            <span className='text-gray-700 text-sm'>Time: {format(new Date(bid.bidTime), 'dd MMM yyyy h:mm a')}</span>
            {/* cấu hình xong nó sẽ về dạng này: 13 Oct 2024 11:05 AM */}
        </div>
        <div className='flex flex-col text-right'>
            <div className='text-xl font-semibold'>${numberWithCommas(bid.amount)}</div>
            <div className='flex flex-row items-center'>
                <span>{getBidInfo().text}</span>
            </div>
        </div>
    </div>
  )
}
