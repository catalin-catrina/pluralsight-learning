import {defineField, defineType} from 'sanity'
import {CalendarIcon} from '@sanity/icons'

export const eventType = defineType({
  name: 'event',
  title: 'Event',
  icon: CalendarIcon,
  type: 'document',
  groups: [
    {name: 'details', title: 'Details'},
    {name: 'editorial', title: 'Editorial'},
  ],
  fields: [
    defineField({
      name: 'name',
      type: 'string',
      group: 'details',
    }),
    defineField({
      name: 'eventType',
      type: 'string',
      options: {
        list: ['in-person', 'virtual'],
        layout: 'radio',
      },
      group: 'details',
    }),
    defineField({
      name: 'slug',
      type: 'slug',
      options: {source: 'name'},
      validation: (rule) => rule.required().error('Required to generate a page on the website'),
      hidden: ({document}) => !document?.name, // hide slug field until name field is filled,
      group: 'details',
    }),
    defineField({
      name: 'date',
      type: 'datetime',
      group: 'details',
    }),
    defineField({
      name: 'doorsopen',
      description: 'Number of minutes before start time for admission',
      type: 'number',
      initialValue: 60,
      group: 'details',
    }),
    defineField({
      name: 'venue',
      type: 'reference',
      to: [{type: 'venue'}],
      readOnly: ({value, document}) => !value && document?.eventType === 'virtual',
      validation: (rule) =>
        rule.custom((value, context) => {
          if (value && context?.document?.eventType === 'virtual') {
            return 'Only in-person events can have a venue'
          }

          return true
        }),
      group: 'details',
    }),
    defineField({
      name: 'headline',
      type: 'reference',
      to: [{type: 'artist'}],
      group: 'details',
    }),
    defineField({
      name: 'image',
      type: 'image',
      group: 'editorial',
    }),
    defineField({
      name: 'details',
      type: 'array',
      of: [{type: 'block'}],
      group: 'editorial',
    }),
    defineField({
      name: 'tickets',
      type: 'url',
      group: 'details',
    }),
  ],
  // preview property defines customizes how the document (sql row) shows up in the list inside sanity localhost
  // the simple version
  preview: {
    select: {
      title: 'name',
      subtitle: 'headline.name',
      media: 'image',
    },
  },
  // the longer version with modifying how data appears in preview
  // preview: {
  //   select: {
  //     name: 'name',
  //     venue: 'venue.name',
  //     artist: 'headline.name',
  //     date: 'date',
  //     image: 'image',
  //   },
  //   prepare({name, venue, artist, date, image}) {
  //     const nameFormatted = name || 'Untitled event'
  //     const dateFormatted = date
  //       ? new Date(date).toLocaleDateString(undefined, {
  //           month: 'short',
  //           day: 'numeric',
  //           year: 'numeric',
  //           hour: 'numeric',
  //           minute: 'numeric',
  //         })
  //       : 'No date'

  //     return {
  //       title: artist ? `${nameFormatted} (${artist})` : nameFormatted,
  //       subtitle: venue ? `${dateFormatted} at ${venue}` : dateFormatted,
  //       media: image || CalendarIcon,
  //     }
  //   },
  // },
})